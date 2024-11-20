using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DischargeSummaryWebAPIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientDataController : ControllerBase
    {
        //private readonly UserDataRepository1 _userDataRepository = new UserDataRepository1();
        //[HttpGet("ptnumber/{ptNo}")]
        //public ActionResult<PatientData> GetPatientData(string ptNo)
        //{
        //    UserDataRepository1 _userDataRepository = new UserDataRepository1();
        //    try
        //    {
        //        PatientData patientData = _userDataRepository.GetDataOfPatient(ptNo);
        //        if (patientData.ipNum.Count == 0)
        //        {
        //            return NotFound("There is no patient with that ID.");
        //        }
        //        var dictionary = new Dictionary<string, object>();
        //        dictionary["data"] = patientData.ipNum;
        //        return Ok(patientData.ipNum);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        [HttpGet("ptnumber/{ptNo}")]
        public ActionResult<PatientData> GetPatientNumber(string ptNo)
        {
            try
            {
                var _userDataRepository = new UserDataRepository();
                List<IpNumber> ipNumbers = _userDataRepository.GetDataOfPatient(ptNo);

                if (ipNumbers.Count == 0)
                {
                    return NotFound("There is no patient with that ID.");
                }
                var dictionary = new Dictionary<string, object>();
                dictionary["data"] = ipNumbers;
                return Ok(ipNumbers);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{ipNumber}")]
        public IActionResult GetPatientData(string ipNumber)
        {
            var _repository = new UserDataRepository();
            try
            {
                var patientData = _repository.getDataOfPatient(ipNumber);
                return Ok(patientData);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
