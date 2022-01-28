using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingApp.Context;
using BookingApp.Data;
using Microsoft.AspNetCore.Authorization;
using BookingApp.Models;

namespace BookingApp.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly BookingContext _context;

        public ReservationsController(BookingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookingContext = _context.Reservations.Include(r => r.Client).Include(r => r.Room);
            return View(await bookingContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomId,ClientId,DateFrom,DateTo,DatePayment,IsPaid")] Reservation reservation)
        {                                                                       
            if (ModelState.IsValid)
            {
                double totalDay =1+ reservation.DateTo.Subtract(reservation.DateFrom).TotalDays;
                reservation.NumberOfDays = (int)totalDay;
                Room room = GetRoom(reservation.RoomId);
          
                decimal totalPrice = (int)totalDay * room.NumbefOfPeople * room.Price;//dodane
                reservation.TotalPrice = totalPrice;
               
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", reservation.ClientId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber", reservation.RoomId);
            return View(reservation);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", reservation.ClientId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber", reservation.RoomId);
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,ClientId,DateFrom,DateTo,DatePayment,IsPaid")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    double totalDay =1+ reservation.DateTo.Subtract(reservation.DateFrom).TotalDays;
                    reservation.NumberOfDays = (int)totalDay;
                    decimal totalPrice = (int)totalDay * reservation.Room.NumbefOfPeople * reservation.Room.Price;//dodane
                    reservation.TotalPrice = totalPrice;
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", reservation.ClientId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber", reservation.RoomId);
            return View(reservation);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }

        private Room GetRoom(int id)
        {
            return _context.Rooms.Where(e => e.Id == id).Single();
        }
    }
}
