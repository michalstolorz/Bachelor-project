﻿using ComputerService.Core.Dto.Request;
using ComputerService.Core.Validators;
using ComputerService.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using AutoMapper;
using System.Collections.Generic;

namespace ComputerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegistrationController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Register new user to the system
        /// </summary>
        /// <param name="request">Request with new user email, password, first name, last name and phone number</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Created user id</returns>
        [HttpPost("registerUser")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RegisterUser(RegisterRequest request, CancellationToken cancellationToken)
        {
            var validator = new RegisterRequestValidator();
            await validator.ValidateAndThrowAsync(request, null, cancellationToken);

            var userToCreate = _mapper.Map<User>(request);

            var creatingUserResult = await _userManager.CreateAsync(userToCreate, request.Password);
            if (!creatingUserResult.Succeeded)
            {
                return BadRequest(creatingUserResult.Errors);
            }

            return Ok(userToCreate.Id);
        }
    }
}
