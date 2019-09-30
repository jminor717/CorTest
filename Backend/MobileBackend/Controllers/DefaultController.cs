using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBackend.AzureControlers;
using MobileBackend.corMonitoring;

namespace MobileBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        public AzureNotifications azureComunications = new AzureNotifications("Endpoint=sb://cor-test.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=n/QrPMw/COwX7rBtd7/+lWewVcmI9oO9/4lBsn3MLYs=", "COR-test-notifications");
        public InstrumentAdder Adder = new InstrumentAdder();
    }
}