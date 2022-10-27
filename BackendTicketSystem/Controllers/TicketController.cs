using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendTicketSystem.CustomModels;
using BackendTicketSystem.Data;
using BackendTicketSystem.Helpers;
using BackendTicketSystem.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty Tickets, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendTicketSystem.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly BackendTicketSystemContext _db = new BackendTicketSystemContext();

        [HttpGet("Tickets")]
        public ApiOutput<List<TicketCustomModel>> Tickets(int skip = 0, int limit = 10)
        {
            try
            {
                var result = new ApiOutput<List<TicketCustomModel>>();

                var ticketList = _db.Tickets.Where(x => x.Status.KeyName == "Active");

                var tickets = ticketList.Select(x => new TicketCustomModel
                {
                    Id = x.Id,
                    Summary = x.Summary,
                    Description =x.Description,
                    PriorityId = x.PriorityId,
                    PriorityName = x.Priority.Name,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project.Name,
                    TicketTypeId = x.TicketTypeId,
                    TicketTypeName = x.TicketType.Name,
                    Severity = x.Severity,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Name,
                    Version = x.Version
                }).OrderByDescending(x => x.Id).Skip(skip).Take(limit).ToList();

                result.Success = true;
                result.Message = "Fetch data successfully!";
                result.Data = tickets;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<List<TicketCustomModel>>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPost("TicketCreate")]
        public ApiOutput<CreateTicketCustomModel> Post([FromBody] CreateTicketCustomModel ticket)
        {

            try
            {
                var result = new ApiOutput<CreateTicketCustomModel>();

                // check duplicate name
                if (_db.Tickets.Any(x => x.Summary.ToLower() == ticket.Summary.ToLower()))
                {
                    result.Success = false;
                    result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                    result.Data = null;
                    return result;
                }

                //new Ticket
                var newTicket = new Ticket
                {
                    Summary = ticket.Summary,
                    Description = ticket.Description,
                    PriorityId=ticket.PriorityId,
                    ProjectId = ticket.ProjectId,
                    TicketTypeId = ticket.TicketTypeId,
                    Severity = ticket.Severity,
                    //CreatedBy = GlobalFunction.GetCurrentUserId(),
                    CreatedBy = 11,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                    Version = 1,
                    StatusId = ticket.StatusId
                };
                _db.Tickets.Add(newTicket);
                _db.SaveChanges();

                result.Success = true;
                result.Message = "Created successfully!";
                result.Data = ticket;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<CreateTicketCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPut("TicketUpdate")]
        public ApiOutput<UpdateTicketCustomModel> Put([FromBody] UpdateTicketCustomModel ticket)
        {
            try
            {
                var result = new ApiOutput<UpdateTicketCustomModel>();

                var currentTicket = _db.Tickets.Find(ticket.Id);
                if (currentTicket.Version == ticket.Version)
                {
                    // check duplicate name
                    if (_db.Tickets.Any(x => x.Summary.ToLower() == currentTicket.Summary.ToLower() && x.Id != ticket.Id))
                    {
                        result.Success = false;
                        result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                        result.Data = null;
                        return result;
                    }

                    // update Ticket
                    currentTicket.Summary = ticket.Summary;
                    currentTicket.Description = ticket.Description;
                    currentTicket.PriorityId = ticket.PriorityId;
                    currentTicket.ProjectId = ticket.ProjectId;
                    currentTicket.TicketTypeId = ticket.TicketTypeId;
                    currentTicket.Severity = ticket.Severity;
                    currentTicket.StatusId = ticket.StatusId;
                    currentTicket.ModifiedBy = 11;
                    //currentTicket.ModifiedBy = DefaultFuntion.GetCurrentUserId();
                    currentTicket.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                    currentTicket.Version = ticket.Version + 1;
                    _db.SaveChanges();

                    result.Success = true;
                    result.Message = "Updated successfully!";
                    result.Data = ticket;
                    return result;
                }

                result.Success = false;
                result.Message = Resource.ValidationMessage_WrongRecordVersion;
                result.Data = null;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<UpdateTicketCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpGet("TicketPriorities")]
        public ApiOutput<List<TicketPriorityCustomModel>> TicketPriorities(int skip = 0, int limit = 10)
        {
            try
            {
                var result = new ApiOutput<List<TicketPriorityCustomModel>>();

                var ticketPriorityList = _db.GlobalParams.Where(x => x.Status.KeyName == "Active" && x.Type == "TicketxxxPriority");

                var ticketPriorities = ticketPriorityList.Select(x => new TicketPriorityCustomModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderByDescending(x => x.Id).Skip(skip).Take(limit).ToList();

                result.Success = true;
                result.Message = "Fetch data successfully!";
                result.Data = ticketPriorities;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<List<TicketPriorityCustomModel>>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }
    }
}

