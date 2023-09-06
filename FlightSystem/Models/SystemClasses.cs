using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FlightSystem.Models
{
    public class FlightBooking
    {
        [Key]
        public int BookingId { get; set; }
        [DisplayName("Flight Class")]
        public string FlightClass { get; set; }
        [DisplayName("Trip Type")]
        public string TripType { get; set; }
        [DisplayName("Number of Passengers")]
        public int NumberOfPassenger { get; set; }
        [DataType(DataType.DateTime)]
        
        public DateTime DateOfBooking { get; set; }
        public string Status { get; set; }
        [DisplayName("Total Cost")]
        public double TotalCost { get; set; }
        [DisplayName("Flight No")]
        public int FlightId { get; set; }
        public Flight flight { get; set; }
        public string Id { get; set; }
        public ApplicationUser user { get; set; }
        [DisplayName("Food Ordering")]
        public string FoodOrdering { get; set; }

        private ApplicationDbContext db = new ApplicationDbContext();
        public Flight getFlight()
        {
            var flight = db.flights.Where(F => F.FlightId == FlightId).FirstOrDefault();
            return flight;
        }
        public double CalcCost()
        {
            double TotalCost;
            if (TripType == "Single")
            {
                if(FlightClass=="First")
                {
                    TotalCost = NumberOfPassenger * getFlight().firstClassCost;
                }
                else if(FlightClass=="Economic")
                {
                    TotalCost = NumberOfPassenger * (getFlight().firstClassCost * 0.5+ getFlight().firstClassCost);
                }
                else
                {
                    TotalCost=NumberOfPassenger* (getFlight().firstClassCost * 0.7+ getFlight().firstClassCost);
                }
            }
            else
            {
                if (FlightClass == "First")
                {
                    TotalCost = 2*(NumberOfPassenger * getFlight().firstClassCost);
                }
                else if (FlightClass == "Economic")
                {
                    TotalCost = 2*(NumberOfPassenger * (getFlight().firstClassCost * 0.5 + getFlight().firstClassCost));
                }
                else
                {
                    TotalCost = 2*(NumberOfPassenger * (getFlight().firstClassCost * 0.7 + getFlight().firstClassCost));
                }
            }
            return TotalCost;
        }
        
        public enum FlightClassList
        {
            First,
            Economic,
            Business
        }
        public enum TripTypeList
        {
            Single,
            Round
        }
        public enum FlightFood
        {
            Breakfast,
            Lunch,
            Dinner
        }
    }
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Date of Flight")]
        public DateTime DateOfFlight { get; set; }
        [DisplayName("Cost of First Class")]
        public double firstClassCost { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime Time { get; set; }
        public ICollection<FlightBooking> flightBookings { get; set; }
    }
}