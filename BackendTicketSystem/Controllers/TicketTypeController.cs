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
    public class TicketTypeController : ControllerBase
    {
        private BackendTicketSystemContext _db = new BackendTicketSystemContext();

        [HttpGet("TicketTypes")]
        public ApiOutput<List<TicketTypeCustomModel>> TicketTypes(int skip = 0, int limit = 10)
        {
            try
            {
                var result = new ApiOutput<List<TicketTypeCustomModel>>();

                var ticketypetList = _db.TicketTypes.Where(x => x.Status.KeyName == "Active");

                var ticketTypes = ticketypetList.Select(x => new TicketTypeCustomModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Memo = x.Memo,
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
                result.Data = ticketTypes;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<List<TicketTypeCustomModel>>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPost("TicketTypeCreate")]
        public ApiOutput<CreateTicketTypeCustomModel> Post([FromBody] CreateTicketTypeCustomModel ticketType)
        {

            try
            {
                var result = new ApiOutput<CreateTicketTypeCustomModel>();

                // check duplicate name
                if (_db.TicketTypes.Any(x => x.Name.ToLower() == ticketType.Name.ToLower()))
                {
                    result.Success = false;
                    result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                    result.Data = null;
                    return result;
                }

                //new Ticket
                var newTicketType = new TicketType
                {
                    Name = ticketType.Name,
                    Memo = ticketType.Memo,
                    //CreatedBy = GlobalFunction.GetCurrentUserId(),
                    CreatedBy = 11,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                    Version = 1,
                    StatusId = ticketType.StatusId
                };
                _db.TicketTypes.Add(newTicketType);
                _db.SaveChanges();

                result.Success = true;
                result.Message = "Created successfully!";
                result.Data = ticketType;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<CreateTicketTypeCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }

        [HttpPut("TicketTypeUpdate")]
        public ApiOutput<UpdateTicketTypeCustomModel> Put([FromBody] UpdateTicketTypeCustomModel ticketType)
        {
            try
            {
                var result = new ApiOutput<UpdateTicketTypeCustomModel>();

                var currentTicketType = _db.TicketTypes.Find(ticketType.Id);
                if (currentTicketType.Version == ticketType.Version)
                {
                    // check duplicate name
                    if (_db.TicketTypes.Any(x => x.Name.ToLower() == currentTicketType.Name.ToLower() && x.Id != ticketType.Id))
                    {
                        result.Success = false;
                        result.Message = string.Format(Resource.ValidationMessage_Duplicate, Resource.Name);
                        result.Data = null;
                        return result;
                    }

                    // update Ticket
                    currentTicketType.Name = ticketType.Name;
                    currentTicketType.Memo = ticketType.Memo;
                    currentTicketType.StatusId = ticketType.StatusId;
                    currentTicketType.ModifiedBy = 11;
                    //currentTicket.ModifiedBy = DefaultFuntion.GetCurrentUserId();
                    currentTicketType.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                    currentTicketType.Version = ticketType.Version + 1;
                    _db.SaveChanges();

                    result.Success = true;
                    result.Message = "Updated successfully!";
                    result.Data = ticketType;
                    return result;
                }

                result.Success = false;
                result.Message = Resource.ValidationMessage_WrongRecordVersion;
                result.Data = null;
                return result;
            }
            catch (Exception ex)
            {
                var result = new ApiOutput<UpdateTicketTypeCustomModel>();
                result.Success = false;
                result.Message = ex.Message.ToString();
                result.Data = null;
                return result;
            }
        }
    }
}

