using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;

using Web.API.Models;
using Data.Common.Abstract;
using Data.Repository;
using Data.Common.Model;
using System.Net.Http;
using System.Net;

namespace Web.API.Controllers
{
    /// <summary>
    /// Currencies
    /// </summary>
    [RoutePrefix("api/organization")]
    public class OrganizationController : ControllerBase
    {
        public OrganizationController(IStoreFactory store):base (store)
        {
        }

        /// <summary>
        /// Get all organizations
        /// </summary>
        /// <returns></returns>
        [Route("", Name = "GetOrganizations")]
        public async Task<IHttpActionResult> GetOrganizations()
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var items = await rep.OrganizationRepository.GetAsync();
                if (items == null)
                {
                    return NotFound();
                }

                var result = new GetOrganizationsResult()
                {
                    PostURL = Url.Link("PostOrganization", null),
                    Organizations = items.AsDTO(Url, false)
                };

                return Ok(result);
            }
        }


        /// <summary>
        /// Get single organization by name
        /// </summary>
        /// <param name="name">ISO-code</param>
        /// <returns></returns>
        [Route("{name}", Name = "Organization")]
        public async Task<IHttpActionResult> GetOrganization(string name)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.OrganizationRepository.GetAsync(name);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item.AsOrganizationDTO(Url));
            }
        }


        /// <summary>
        /// Add new organization
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("", Name = "PostOrganization")]
        [Authorize]
        public async Task<IHttpActionResult> PostOrganization([FromBody] OrganizationDetailsDTO organization)
        {
            if (organization == null)
            {
                return BadRequest($"Request content is empty");
            }
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.OrganizationRepository.GetAsync(organization.Name);
                if (item != null)
                {
                    return BadRequest("Organization already exists");
                }

                var newItem = new Organization()
                {
                    Name = organization.Name.ToUpper(),
                    Description = organization.Description,
                    Countries = new List<Country>(),
                };

                if (organization.Countries != null)
                {
                    foreach (var cDto in organization.Countries)
                    {
                        var c = await rep.CountryRepository.GetAsync(cDto.IsoCode);
                        if (c == null)
                        {
                            return BadRequest($"Country {cDto.IsoCode} not found");
                        }
                        newItem.Countries.Add(c);
                    }
                }
                
                newItem = rep.OrganizationRepository.Add(newItem);

                await rep.CompleteAsync();

                var result = newItem.AsOrganizationDTO(Url);
                var response = Request.CreateResponse(HttpStatusCode.Created, result);
                response.Headers.Location = new Uri(result.GetUrl);
                return ResponseMessage(response);
            }
        }


        /// <summary>
        /// Update organization
        /// </summary>
        /// <param name="name"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        [Route("{name}", Name = "PutOrganization")]
        [HttpPut]
        [Authorize]
        public async Task<IHttpActionResult> UpdateOrganization(string name, 
            [FromBody] OrganizationDetailsDTO organization)
        {
            if (organization == null)
            {
                return BadRequest($"Request content is empty");
            }
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.OrganizationRepository.GetAsync(name);
                if (item == null)
                {
                    return NotFound();
                }
                
                item.Name = organization.Name.ToUpper();
                item.Description = organization.Description;

                await rep.CompleteAsync();

                var result = item.AsOrganizationDTO(Url);
                var response = Request.CreateResponse(HttpStatusCode.Created, result);
                response.Headers.Location = new Uri(result.GetUrl);
                return ResponseMessage(response);
            }
        }


        /// <summary>
        /// Delete organization
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("{name}", Name = "DeleteOrganization")]
        [HttpDelete]
        [Authorize]
        public async Task<IHttpActionResult> DeleteOrganization(string name)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.OrganizationRepository.GetAsync(name);
                if (item == null)
                {
                    return NotFound();
                }

                rep.OrganizationRepository.Remove(item);

                await rep.CompleteAsync();

                return Ok();
            }
        }
    }
}