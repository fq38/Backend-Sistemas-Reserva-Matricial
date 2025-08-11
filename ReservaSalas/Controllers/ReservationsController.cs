using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaSalas.Data;
using ReservaSalas.Models;
using System;
using ReservaSalas.Dto;

namespace ReservaSalas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _context.Reservations.Include(r => r.Room).ThenInclude(s => s.Location).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservations
                                        .Include(r => r.Room)
                                        .ThenInclude(room => room.Location)
                                        .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }
            return reservation;
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(CreateReservationDto reservationDto)
        {
            // Validação extra no servidor
            if (reservationDto.StartTime >= reservationDto.EndTime)
            {
                return BadRequest("A data de início deve ser anterior à data de fim.");
            }

            // A validação de conflito. O model binder do ASP.NET Core já converte
            // a string ISO para um DateTime com Kind=UTC, então a comparação direta funciona.
            var conflict = await _context.Reservations
    .AnyAsync(r => r.RoomId == reservationDto.RoomId &&
                   r.StartTime < reservationDto.EndTime &&
                   r.EndTime > reservationDto.StartTime);

            if (conflict)
            {
                return Conflict("Conflito de horário. Já existe uma reserva para esta sala no período solicitado.");
            }

            
            var newReservation = new Reservation
            {
                RoomId = reservationDto.RoomId,
                StartTime = reservationDto.StartTime, // Já está em UTC
                EndTime = reservationDto.EndTime,     // Já está em UTC
                Responsible = reservationDto.Responsible,
                HasCoffee = reservationDto.HasCoffee,
                NumberOfPeopleForCoffee = reservationDto.HasCoffee ? reservationDto.NumberOfPeopleForCoffee : null,
                Description = reservationDto.Description
            };

            _context.Reservations.Add(newReservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservation), new { id = newReservation.Id }, newReservation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, CreateReservationDto reservationDto)
        {
            if (reservationDto.StartTime >= reservationDto.EndTime)
            {
                return BadRequest("A data de início deve ser anterior à data de fim.");
            }

            var reservationToUpdate = await _context.Reservations.FindAsync(id);
            if (reservationToUpdate == null) return NotFound("Reserva não encontrada.");

            // Lógica de validação de conflito para ATUALIZAÇÃO
            var conflict = await _context.Reservations
    .AnyAsync(r => r.Id != id &&
                   r.RoomId == reservationDto.RoomId &&
                   r.StartTime < reservationDto.EndTime &&
                   r.EndTime > reservationDto.StartTime);

            if (conflict)
            {
                return Conflict("Conflito de horário. Já existe outra reserva para esta sala no período solicitado.");
            }

            
            reservationToUpdate.RoomId = reservationDto.RoomId;
            reservationToUpdate.StartTime = reservationDto.StartTime; // Já está em UTC
            reservationToUpdate.EndTime = reservationDto.EndTime;     // Já está em UTC
            reservationToUpdate.Responsible = reservationDto.Responsible;
            reservationToUpdate.HasCoffee = reservationDto.HasCoffee;
            reservationToUpdate.NumberOfPeopleForCoffee = reservationDto.HasCoffee ? reservationDto.NumberOfPeopleForCoffee : null;
            reservationToUpdate.Description = reservationDto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }



    }
}
