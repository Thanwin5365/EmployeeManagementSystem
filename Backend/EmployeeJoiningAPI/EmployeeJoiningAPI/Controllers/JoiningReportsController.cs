using EmployeeJoiningAPI.Models;
using EmployeeJoiningAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmployeeJoiningAPI.Controllers
{
    [ApiController]
    [Route("joining-reports")]
    public class JoiningReportsController : ControllerBase
    {
        private readonly IJoiningReportService _service;

        public JoiningReportsController(
            IJoiningReportService service)
        {
            _service = service;
        }


        // =====================================================
        // GET ALL JOINING REPORTS
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetJoiningReports()
        {
            try
            {
                var reports =
                    await _service.GetAllReportsAsync();

                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // GET JOINING REPORT BY ID
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetJoiningReport(int id)
        {
            try
            {
                var report =
                    await _service.GetReportByIdAsync(id);

                if (report == null)
                {
                    return NotFound(new
                    {
                        message = "Joining report not found."
                    });
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // GET JOINING REPORTS BY EMPLOYEE ID
        // =====================================================

        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult>
            GetJoiningReportsByEmployee(int employeeId)
        {
            try
            {
                var reports =
                    await _service.GetReportsByEmployeeAsync(
                        employeeId);

                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // CREATE JOINING REPORT
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateJoiningReport(
            [FromBody] JoiningReport joiningReport)
        {
            try
            {
                if (joiningReport == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Joining report data is required."
                    });
                }

                var result =
                    await _service.CreateReportAsync(
                        joiningReport);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LEVEL 1 - PENDING
        // =====================================================
        // KEEPING OLD ROUTE:
        // GET /joining-reports/pending
        // =====================================================

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingReports()
        {
            try
            {
                var reports =
                    await _service
                        .GetPendingLevel1ReportsAsync();

                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LEVEL 1 - PENDING ALIAS
        // =====================================================
        // Also supports:
        // GET /joining-reports/pending-level1
        // =====================================================

        [HttpGet("pending-level1")]
        public async Task<IActionResult>
            GetPendingLevel1Reports()
        {
            try
            {
                var reports =
                    await _service
                        .GetPendingLevel1ReportsAsync();

                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        
        // LEVEL 2 - PENDING
        

        [HttpGet("pending-level2/{level2ManagerId:int}")]
        public async Task<IActionResult>
            GetLevel2PendingReports(
                int level2ManagerId)
        {
            try
            {
                if (level2ManagerId <= 0)
                {
                    return BadRequest(new
                    {
                        message =
                            "Level 2 Manager ID is required."
                    });
                }

                var reports =
                    await _service
                        .GetPendingLevel2ReportsAsync(
                            level2ManagerId);

                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LEVEL 1 - APPROVE
        // =====================================================
        // OLD WORKING ROUTE:
        // PUT /joining-reports/{id}/approve
        // =====================================================

        [HttpPut("{id:int}/approve")]
        public async Task<IActionResult> ApproveLevel1(
            int id,
            [FromBody] ApproveRequest? request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid report ID."
                    });
                }

                var result =
                    await _service
                        .ApproveLevel1Async(
                            id,
                            request?.ApprovedBy);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LEVEL 1 - APPROVE ALIAS
        // =====================================================
        // PUT /joining-reports/{id}/level1/approve
        // =====================================================

        [HttpPut("{id:int}/level1/approve")]
        public async Task<IActionResult> ApproveLevel1Alias(
            int id,
            [FromBody] ApproveRequest? request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid report ID."
                    });
                }

                var managerId = request?.ApprovedBy;

                if (managerId == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Level 1 Manager ID is required."
                    });
                }

                var result =
                    await _service
                        .ApproveLevel1Async(
                            id,
                            managerId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LEVEL 1 - REJECT
        // =====================================================
        // OLD WORKING ROUTE:
        // PUT /joining-reports/{id}/reject
        //
        // Accepts BOTH:
        //
        // "bad"
        //
        // OR
        //
        // {
        //     "reason": "bad"
        // }
        // =====================================================

        [HttpPut("{id:int}/reject")]
        public async Task<IActionResult> RejectLevel1(
            int id,
            [FromBody] JsonElement request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid report ID."
                    });
                }

                string? reason = null;

                // -------------------------------------------------
                // Angular sends:
                // "bad"
                // -------------------------------------------------

                if (request.ValueKind ==
                    JsonValueKind.String)
                {
                    reason = request.GetString();
                }

                // -------------------------------------------------
                // Angular sends:
                // { "reason": "bad" }
                // -------------------------------------------------

                else if (request.ValueKind ==
                         JsonValueKind.Object)
                {
                    if (request.TryGetProperty(
                        "reason",
                        out JsonElement reasonElement))
                    {
                        reason = reasonElement.GetString();
                    }
                }

                if (string.IsNullOrWhiteSpace(reason))
                {
                    return BadRequest(new
                    {
                        message =
                            "Rejection reason is required."
                    });
                }

                var result =
                    await _service
                        .RejectLevel1Async(
                            id,
                            reason.Trim());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LEVEL 1 - REJECT ALIAS
        // =====================================================
        // PUT /joining-reports/{id}/level1/reject
        // =====================================================

        [HttpPut("{id:int}/level1/reject")]
        public async Task<IActionResult>
            RejectLevel1Alias(
                int id,
                [FromBody] RejectRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid report ID."
                    });
                }

                if (request == null ||
                    string.IsNullOrWhiteSpace(
                        request.Reason))
                {
                    return BadRequest(new
                    {
                        message =
                            "Rejection reason is required."
                    });
                }

                var result =
                    await _service
                        .RejectLevel1Async(
                            id,
                            request.Reason.Trim());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LEVEL 2 - APPROVE
        // =====================================================
        // OLD WORKING ROUTE:
        // PUT /joining-reports/{id}/approve-level2
        // =====================================================

        [HttpPut("{id:int}/approve-level2")]
        public async Task<IActionResult> ApproveLevel2(
            int id,
            [FromBody] ApproveRequest? request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid report ID."
                    });
                }

                if (request?.ApprovedBy == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Level 2 Manager ID is required."
                    });
                }

                var result =
                    await _service
                        .ApproveLevel2Async(
                            id,
                            request.ApprovedBy.Value);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LEVEL 2 - APPROVE ALIAS
        // =====================================================

        [HttpPut("{id:int}/level2/approve")]
        public async Task<IActionResult>
            ApproveLevel2Alias(
                int id,
                [FromBody] ApproveRequest? request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid report ID."
                    });
                }

                if (request?.ApprovedBy == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Level 2 Manager ID is required."
                    });
                }

                var result =
                    await _service
                        .ApproveLevel2Async(
                            id,
                            request.ApprovedBy.Value);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LEVEL 2 - REJECT
        // =====================================================
        // OLD WORKING ROUTE:
        // PUT /joining-reports/{id}/reject-level2
        // =====================================================

        [HttpPut("{id:int}/reject-level2")]
        public async Task<IActionResult> RejectLevel2(
            int id,
            [FromBody] Level2RejectRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid report ID."
                    });
                }

                if (request == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Request data is required."
                    });
                }

                if (request.ManagerId <= 0)
                {
                    return BadRequest(new
                    {
                        message =
                            "Level 2 Manager ID is required."
                    });
                }

                if (string.IsNullOrWhiteSpace(
                    request.Reason))
                {
                    return BadRequest(new
                    {
                        message =
                            "Rejection reason is required."
                    });
                }

                var result =
                    await _service
                        .RejectLevel2Async(
                            id,
                            request.ManagerId,
                            request.Reason.Trim());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LEVEL 2 - REJECT ALIAS
        // =====================================================

        [HttpPut("{id:int}/level2/reject")]
        public async Task<IActionResult>
            RejectLevel2Alias(
                int id,
                [FromBody] Level2RejectRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid report ID."
                    });
                }

                if (request == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Request data is required."
                    });
                }

                if (request.ManagerId <= 0)
                {
                    return BadRequest(new
                    {
                        message =
                            "Level 2 Manager ID is required."
                    });
                }

                if (string.IsNullOrWhiteSpace(
                    request.Reason))
                {
                    return BadRequest(new
                    {
                        message =
                            "Rejection reason is required."
                    });
                }

                var result =
                    await _service
                        .RejectLevel2Async(
                            id,
                            request.ManagerId,
                            request.Reason.Trim());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // HR - PENDING
        // =====================================================

        [HttpGet("pending-hr")]
        public async Task<IActionResult>
            GetPendingHrReports()
        {
            try
            {
                var reports =
                    await _service
                        .GetPendingHrReportsAsync();

                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // HR - APPROVE
        // =====================================================

        [HttpPut("{id:int}/hr/approve")]
        public async Task<IActionResult> ApproveHr(
            int id,
            [FromBody] ApproveRequest? request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid report ID."
                    });
                }

                if (request?.ApprovedBy == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "HR Manager ID is required."
                    });
                }

                var result =
                    await _service
                        .ApproveHrAsync(
                            id,
                            request.ApprovedBy.Value);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // HR - REJECT
        // =====================================================

        [HttpPut("{id:int}/hr/reject")]
        public async Task<IActionResult> RejectHr(
            int id,
            [FromBody] HrRejectRequest request)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid report ID."
                    });
                }

                if (request == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Request data is required."
                    });
                }

                if (request.ManagerId <= 0)
                {
                    return BadRequest(new
                    {
                        message =
                            "HR Manager ID is required."
                    });
                }

                if (string.IsNullOrWhiteSpace(
                    request.Reason))
                {
                    return BadRequest(new
                    {
                        message =
                            "Rejection reason is required."
                    });
                }

                var result =
                    await _service
                        .RejectHrAsync(
                            id,
                            request.ManagerId,
                            request.Reason.Trim());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
        // =====================================================
        // GET APPROVAL HISTORY
        // =====================================================

        [HttpGet("{id:int}/approval-history")]
        public async Task<IActionResult> GetApprovalHistory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid joining report ID."
                    });
                }

                var history =
                    await _service.GetApprovalHistoryAsync(id);

                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }


    // =========================================================
    // REQUEST MODELS
    // =========================================================

    public class ApproveRequest
    {
        public int? ApprovedBy { get; set; }
    }


    public class RejectRequest
    {
        public string Reason { get; set; } =
            string.Empty;
    }


    public class Level2RejectRequest
    {
        public int ManagerId { get; set; }

        public string Reason { get; set; } =
            string.Empty;
    }


    public class HrRejectRequest
    {
        public int ManagerId { get; set; }

        public string Reason { get; set; } =
            string.Empty;
    }
}