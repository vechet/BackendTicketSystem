using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using BackendTicketSystem.CustomModels;
using BackendTicketSystem.Data;
using BackendTicketSystem.Helpers;
using BackendTicketSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;

// For more information on enabling Web API for empty Tickets, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendTicketSystem.Controllers
{
    [Authorize]
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

                var tickets = _db.Tickets.Where(x => x.Status.KeyName == "Active").Select(x => new TicketCustomModel()
                {
                    Id = x.Id,
                    Subject = x.Subject,
                    PriorityId = x.PriorityId,
                    PriorityName = x.Priority.Name,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project.Name,
                    TicketTypeId = x.TicketTypeId,
                    TicketTypeName = x.TicketType.Name,
                    Severity = x.Severity,
                    TransactionType=x.TransactionType,
                    TransactionTypeName=_db.GlobalParams.FirstOrDefault(z=>z.Id==x.TransactionType).Name,
                    DueDate =x.DueDate,
                    OpennedBy=x.OpennedBy,
                    OpennedDate=x.OpennedDate,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Name,
                    Version = x.Version,
                    TicketActionList = _db.TicketActions.Where(z => z.TicketId == x.Id).Select(z => new TicketActionCustomModel
                    {
                        Id =z.Id,
                        TicketId = z.TicketId,
                        Description=z.Description,
                        UserId = z.UserId,
                        UserName = _db.UserAccounts.FirstOrDefault(u => u.Id == z.UserId).FullName,
                        TransactionDate = z.TransactionDate
                    }).ToList()
                }).ToList();

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
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var currentUserId = GlobalFunction.GetCurrentUserId(_db,_bearer_token);

                // check duplicate name
                if (_db.Tickets.Any(x => x.Subject.ToLower() == ticket.Subject.ToLower() && x.Status.KeyName == "Active"))
                {
                    result.Success = false;
                    result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                    result.Data = null;
                    return result;
                }

                //new Ticket
                var newTicket = new Ticket
                {
                    Subject = ticket.Subject,
                    PriorityId = ticket.PriorityId,
                    ProjectId = ticket.ProjectId,
                    TicketTypeId = ticket.TicketTypeId,
                    Severity = ticket.Severity ?? false,
                    TransactionType = _db.GlobalParams.FirstOrDefault(x => x.Type == "TicketxxxTransactionType").Id,
                    DueDate = ticket.DueDate,
                    OpennedBy = currentUserId,
                    OpennedDate = GlobalFunction.GetCurrentDateTime(),
                    CreatedBy = currentUserId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                    Version = 1,
                    StatusId = _db.Statuses.FirstOrDefault(x => x.KeyName == "Active").Id,
                };
                _db.Tickets.Add(newTicket);

                var newTicketAction = new TicketAction
                {
                    Description = ticket.Description,
                    UserId = currentUserId,
                    TransactionDate = GlobalFunction.GetCurrentDateTime()

                };
                newTicket.TicketActions.Add(newTicketAction);
                _db.SaveChanges();

                ticket.Id = newTicket.Id;
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

        [HttpPost("TicketUpdate")]
        public ApiOutput<UpdateTicketCustomModel> TicketUpdate([FromBody] UpdateTicketCustomModel ticket)
        {
            try
            {
                var result = new ApiOutput<UpdateTicketCustomModel>();
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var currentUserId = GlobalFunction.GetCurrentUserId(_db, _bearer_token);

                var currentTicket = _db.Tickets.Find(ticket.Id);
                if (currentTicket.Version == ticket.Version)
                {
                    // check duplicate name
                    if (_db.Tickets.Any(x => x.Subject.ToLower() == currentTicket.Subject.ToLower() && x.Id != ticket.Id && x.Status.KeyName == "Active"))
                    {
                        result.Success = false;
                        result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                        result.Data = null;
                        return result;
                    }

                    // update Ticket
                    currentTicket.Subject = ticket.Subject;
                    currentTicket.PriorityId = ticket.PriorityId;
                    currentTicket.ProjectId = ticket.ProjectId;
                    currentTicket.TicketTypeId = ticket.TicketTypeId;
                    currentTicket.Severity = ticket.Severity;
                    currentTicket.TransactionType = ticket.TransactionType;
                    currentTicket.DueDate = ticket.DueDate;
                    currentTicket.StatusId = ticket.StatusId;
                    currentTicket.ModifiedBy = currentUserId;
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

        [HttpGet("TicketTransactionTypes")]
        public ApiOutput<List<TicketTransactionTypesCustomModel>> TicketTransactionTypes(int skip = 0, int limit = 10)
        {
            try
            {
                var result = new ApiOutput<List<TicketTransactionTypesCustomModel>>();

                var ticketTransactionTypeList = _db.GlobalParams.Where(x => x.Status.KeyName == "Active" && x.Type == "TicketxxxTransactionType");

                var ticketTransactionTypes = ticketTransactionTypeList.Select(x => new TicketTransactionTypesCustomModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderByDescending(x => x.Id).Skip(skip).Take(limit).ToList();

                result.Success = true;
                result.Message = "Fetch data successfully!";
                result.Data = ticketTransactionTypes;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<List<TicketTransactionTypesCustomModel>>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpGet("TicketDetail")]
        public ApiOutput<TicketDetailCustomModel> TicketDetail(int id = 0)
        {
            try
            {
                var result = new ApiOutput<TicketDetailCustomModel>();

                var ticketDetail = _db.Tickets.Where(x => x.Status.KeyName == "Active" && x.Id == id).Select(x => new TicketDetailCustomModel()
                {
                    Id = x.Id,
                    Subject = x.Subject,
                    PriorityId = x.PriorityId,
                    PriorityName = x.Priority.Name,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project.Name,
                    TicketTypeId = x.TicketTypeId,
                    TicketTypeName = x.TicketType.Name,
                    Severity = x.Severity,
                    TransactionType = x.TransactionType,
                    TransactionTypeName = _db.GlobalParams.FirstOrDefault(z => z.Id == x.TransactionType).Name,
                    DueDate = x.DueDate,
                    OpennedBy = x.OpennedBy,
                    OpennedDate = x.OpennedDate,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Name,
                    Version = x.Version,
                    TicketActionList = _db.TicketActions.Where(z => z.TicketId == x.Id).Select(z => new TicketActionCustomModel
                    {
                        Id = z.Id,
                        TicketId = z.TicketId,
                        Description = z.Description,
                        UserId = z.UserId,
                        UserName = _db.UserAccounts.FirstOrDefault(u => u.Id == z.UserId).FullName,
                        TransactionDate = z.TransactionDate,
                    }).OrderByDescending(z=>z.Id).ToList()
                }).FirstOrDefault();

                result.Success = true;
                result.Message = "Fetch data successfully!";
                result.Data = ticketDetail;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<TicketDetailCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPost("ReplyTicket")]
        public ApiOutput<ReplyTicketCustomModel> ReplyTicket([FromBody] ReplyTicketCustomModel replyTicket)
        {
            try
            {
                var result = new ApiOutput<ReplyTicketCustomModel>();
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var currentUserId = GlobalFunction.GetCurrentUserId(_db, _bearer_token);

                var newTicketAction = new TicketAction
                {
                    TicketId = replyTicket.TicketId,
                    Description = replyTicket.Description,
                    UserId = currentUserId,
                    TransactionDate = GlobalFunction.GetCurrentDateTime()
                };
                _db.TicketActions.Add(newTicketAction);
                _db.SaveChanges();

                result.Success = true;
                result.Message = "Replied Ticket successfully!";
                result.Data = replyTicket;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<ReplyTicketCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPost("UpdateTicketTransactionType")]
        public ApiOutput<UpdateTicketTransactionTypeCustomModel> UpdateTicketTransactionType([FromBody] UpdateTicketTransactionTypeCustomModel ticket)
        {
            try
            {
                var result = new ApiOutput<UpdateTicketTransactionTypeCustomModel>();
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var currentUserId = GlobalFunction.GetCurrentUserId(_db, _bearer_token);

                var currentTicket = _db.Tickets.Find(ticket.TicketId);
                currentTicket.TransactionType = ticket.TransactionType;
                currentTicket.ModifiedBy = currentUserId;
                currentTicket.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                currentTicket.Version = ticket.Version + 1;
                _db.SaveChanges();

                result.Success = true;
                result.Message = "Updated successfully!";
                result.Data = ticket;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<UpdateTicketTransactionTypeCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPost("TicketDelete")]
        public ApiOutput<DeleteTicketCustomModel> TicketDelete([FromBody] DeleteTicketCustomModel ticket)
        {
            try
            {
                var result = new ApiOutput<DeleteTicketCustomModel>();
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var currentUserId = GlobalFunction.GetCurrentUserId(_db, _bearer_token);

                var currentTicket = _db.Tickets.Find(ticket.Id);
                currentTicket.StatusId = _db.Statuses.FirstOrDefault(x => x.KeyName == "Inactive").Id;
                currentTicket.ModifiedBy = currentUserId;
                currentTicket.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                _db.SaveChanges();

                result.Success = true;
                result.Message = "Ticket delete successfully!";
                result.Data = ticket;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<DeleteTicketCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }
    }
}

