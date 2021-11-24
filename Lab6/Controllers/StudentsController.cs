﻿using Lab6.Controllers;
using Lab6.Data;
using Lab6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {

        private readonly StudentDbContext _context;
        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Get collection of Students.
        /// </summary>
     
        /// <returns>A collection of Students</returns>
        /// <response code="200">Returns a collection of Students</response>
        /// <response code="500">Internal error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            return Ok(await _context.Students.ToListAsync());
        }


        /// <summary>
        /// Get student by ID
        /// </summary>
        /// <param id ="id"></param>
        /// <returns> A Student </returns>
        /// <response code="200">Returns a Student by ID</response>
        /// <response code="500">Internal error</response>response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> GetById(Guid id)
        {
            Student student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // POST: Students
        /// <summary>
        /// Creates a Student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Student
        ///     {
        ///        "FirstName": "Meech",
        ///        "Lastname": "Lamirande",
        ///        "Program" : "CST"
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created Student</returns>
        /// <response code="201">Returns the newly created Student</response>
        /// <response code="400">If the Student is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> CreateAsync([Bind("FirstName,LastName,Program")] StudentBase studentBase)
        {
            Student student = new Student
            {
                FirstName = studentBase.FirstName,
                LastName = studentBase.LastName,
                Program = studentBase.Program
            };

            _context.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }


        // DELETE: Students/5
        /// <summary>
        /// Deletes a Student.
        /// </summary>
        /// <param id="id"></param>
        /// <response code="202">Student is deleted</response>
        /// <response code="400">If the id is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return Accepted();
        }


        //UPDATE: Students
        /// <summary>
        /// Upserts a Student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Student
        ///     {
        ///        "FirstName": "Meech",
        ///        "Lastname": "Lamirande",
        ///        "Program" : "CST"
        ///     }
        ///
        /// </remarks>
        /// <param id="id"></param>
        /// <returns>An upserted Student</returns>
        /// <response code="200">Returns the updated Student</response>
        /// <response code="201">Returns the newly created Student</response>
        /// <response code="400">If the Student or id is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> Upsert(Guid id, [Bind("FirstName,LastName,Program")] StudentBase studentBase)
        {
         
            //find the student if it exists if it doesn't return not found. 
            Student student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            if(studentBase.FirstName.Length != 0 )
            {
                student.FirstName = studentBase.FirstName;
            }
           if(studentBase.LastName.Length != 0 )
            {
                student.LastName = studentBase.LastName;
            }
            if (studentBase.Program.Length != 0)
            {
                student.Program = studentBase.Program;
            }

          
            _context.Update(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }

   

    }
}
