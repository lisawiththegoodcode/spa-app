using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentTracker.Models
{
    public class AppointmentModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime AppointmentTime { get; set; }

        [UIHint("Client")]
        public CustomerModel Client { get; set; }
        public int ClientId { get; set; }


        [UIHint("Provider")]
        public ServiceProviderModel Provider { get; set; }
        public int ProviderId { get; set; }

        [Required]
        public string Service { get; set; }
    }
}
