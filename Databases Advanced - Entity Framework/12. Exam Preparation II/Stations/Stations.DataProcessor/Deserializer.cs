using System;
using Stations.Data;
using Newtonsoft.Json;
using Stations.Models;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Stations.DataProcessor.Dto.Import;
using Stations.Models.Enums;
using System.Globalization;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Stations.DataProcessor.Dto.Import.Ticket;

namespace Stations.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportStations(StationsDbContext context, string jsonString)
        {
            var stations = JsonConvert.DeserializeObject<StationDto[]>(jsonString);

            var sb = new StringBuilder();
            var validStations = new List<Station>();

            foreach (var dto in stations)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (validStations.Any(s => s.Name == dto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (dto.Town == null)
                {
                    dto.Town = dto.Name;
                }

                var station = new Station()
                {
                    Name = dto.Name,
                    Town = dto.Town
                };

                validStations.Add(station);
                sb.AppendLine(string.Format(SuccessMessage, dto.Name));
            }

            context.Stations.AddRange(validStations);
            context.SaveChanges();

            var result = sb.ToString().Trim();
            return result;
        }

        public static string ImportClasses(StationsDbContext context, string jsonString)
        {
            var seatingClasses = JsonConvert.DeserializeObject<SeatingClassDto[]>(jsonString);

            var sb = new StringBuilder();
            var validSeatingClasses = new List<SeatingClass>();

            foreach (var dto in seatingClasses)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (validSeatingClasses.Any(sc => sc.Name == dto.Name || sc.Abbreviation == dto.Abbreviation))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var seatingClass = new SeatingClass()
                {
                    Name = dto.Name,
                    Abbreviation = dto.Abbreviation
                };

                validSeatingClasses.Add(seatingClass);
                sb.AppendLine(string.Format(SuccessMessage, dto.Name));
            }

            context.SeatingClasses.AddRange(validSeatingClasses);
            context.SaveChanges();

            var result = sb.ToString().Trim();
            return result;
        }

        public static string ImportTrains(StationsDbContext context, string jsonString)
        {
            var trains = JsonConvert.DeserializeObject<TrainDto[]>(jsonString);

            var sb = new StringBuilder();
            var validTrains = new List<Train>();

            foreach (var dto in trains)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (validTrains.Any(t => t.TrainNumber == dto.TrainNumber))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (dto.Type == null)
                {
                    dto.Type = "HighSpeed";
                }

                var type = Enum.Parse<TrainType>(dto.Type);

                var seatsAreValid = dto.Seats.All(IsValid);

                if (!seatsAreValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var seatingClassesAreValid = dto
                    .Seats
                    .All(s => context.SeatingClasses
                        .Any(sc => sc.Name == s.Name && sc.Abbreviation == s.Abbreviation));

                if (!seatingClassesAreValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var trainSeats = dto.Seats.Select(s => new TrainSeat
                {
                    SeatingClass = context.SeatingClasses.SingleOrDefault(sc => sc.Name == s.Name && sc.Abbreviation == s.Abbreviation),
                    Quantity = s.Quantity.Value
                })
                .ToArray();

                var train = new Train()
                {
                    TrainNumber = dto.TrainNumber,
                    Type = type,
                    TrainSeats = trainSeats
                };

                validTrains.Add(train);
                sb.AppendLine(string.Format(SuccessMessage, dto.TrainNumber));
            }

            context.Trains.AddRange(validTrains);
            context.SaveChanges();

            var result = sb.ToString().Trim();
            return result;
        }

        public static string ImportTrips(StationsDbContext context, string jsonString)
        {
            var trips = JsonConvert.DeserializeObject<TripDto[]>(jsonString);

            var sb = new StringBuilder();
            var validTrips = new List<Trip>();

            foreach (var dto in trips)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var train = context.Trains.SingleOrDefault(t => t.TrainNumber == dto.Train);
                var originStation = context.Stations.SingleOrDefault(s => s.Name == dto.OriginStation);
                var destinationStation = context.Stations.SingleOrDefault(s => s.Name == dto.DestinationStation);

                if (train == null || originStation == null || destinationStation == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (dto.Status == null)
                {
                    dto.Status = "OnTime";
                }

                var status = Enum.Parse<TripStatus>(dto.Status);
                var departureTime = DateTime.ParseExact(dto.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var arrivalTime = DateTime.ParseExact(dto.ArrivalTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                TimeSpan timeDifference;
                if (dto.TimeDifference != null)
                {
                    timeDifference = TimeSpan.ParseExact(dto.TimeDifference, "hh\\:mm", CultureInfo.InvariantCulture);
                }

                if (departureTime > arrivalTime)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var trip = new Trip()
                {
                    Train = train,
                    OriginStation = originStation,
                    DestinationStation = destinationStation,
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    Status = status,
                    TimeDifference = timeDifference
                };

                validTrips.Add(trip);
                sb.AppendLine($"Trip from {dto.OriginStation} to {dto.DestinationStation} imported.");
            }

            context.Trips.AddRange(validTrips);
            context.SaveChanges();

            var result = sb.ToString().Trim();
            return result;
        }

        public static string ImportCards(StationsDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(CardDto[]), new XmlRootAttribute("Cards"));
            var deserializedCards = (CardDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();
            var validCards = new List<CustomerCard>();

            foreach (var dto in deserializedCards)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (dto.Type == null)
                {
                    dto.Type = "Normal";
                }

                var cardTyoe = Enum.Parse<CardType>(dto.Type);

                var card = new CustomerCard()
                {
                    Name = dto.Name,
                    Age = dto.Age,
                    Type = cardTyoe
                };

                validCards.Add(card);
                sb.AppendLine(string.Format(SuccessMessage, dto.Name));
            }

            context.AddRange(validCards);
            context.SaveChanges();

            var result = sb.ToString().Trim();
            return result;
        }

        public static string ImportTickets(StationsDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(TicketDto[]), new XmlRootAttribute("Tickets"));
            var deserializedTickets = (TicketDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();

            var validTickets = new List<Ticket>();
            foreach (var ticketDto in deserializedTickets)
            {
                if (!IsValid(ticketDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var departureTime =
                    DateTime.ParseExact(ticketDto.Trip.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                var trip = context.Trips
                    .Include(t => t.OriginStation)
                    .Include(t => t.DestinationStation)
                    .Include(t => t.Train)
                    .ThenInclude(t => t.TrainSeats)
                    .SingleOrDefault(t => t.OriginStation.Name == ticketDto.Trip.OriginStation &&
                                                              t.DestinationStation.Name == ticketDto.Trip.DestinationStation &&
                                                              t.DepartureTime == departureTime);
                if (trip == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                CustomerCard card = null;
                if (ticketDto.Card != null)
                {
                    card = context.Cards.SingleOrDefault(c => c.Name == ticketDto.Card.Name);

                    if (card == null)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }
                }

                var seatingClassAbbreviation = ticketDto.Seat.Substring(0, 2);
                var quantity = int.Parse(ticketDto.Seat.Substring(2));

                var seatExists = trip.Train.TrainSeats
                    .SingleOrDefault(s => s.SeatingClass.Abbreviation == seatingClassAbbreviation && quantity <= s.Quantity);
                if (seatExists == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var seat = ticketDto.Seat;

                var ticket = new Ticket
                {
                    Trip = trip,
                    CustomerCard = card,
                    Price = ticketDto.Price,
                    SeatingPlace = seat
                };

                validTickets.Add(ticket);
                sb.AppendLine(string.Format("Ticket from {0} to {1} departing at {2} imported.",
                    trip.OriginStation.Name,
                    trip.DestinationStation.Name,
                    trip.DepartureTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)));
            }

            context.Tickets.AddRange(validTickets);
            context.SaveChanges();

            var result = sb.ToString().Trim();
            return result;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }
}