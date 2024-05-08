using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vjezba.Model;

public class Client
{
	[Key]
	public int ID { get; set; }

	[Required(ErrorMessage = "Polje Ime je obavezno.")]
	public string FirstName { get; set; }

	[Required(ErrorMessage = "Polje Prezime je obavezno.")]
	public string LastName { get; set; }

	[Required(ErrorMessage = "Polje Email je obavezno.")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Polje Spol je obavezno.")]
	public char Gender { get; set; }

	[Required(ErrorMessage = "Polje Obitavalište je obavezno.")]
	public string Address { get; set; }

	[Required(ErrorMessage = "Polje Tel je obavezno.")]
	public string PhoneNumber { get; set; }

	//[Required(ErrorMessage = "Polje Radni staž je obavezno")]
	//[Range(1, 100, ErrorMessage = "Godine radnog staža moraju biti između 0 i 100")]
	public int WorkingExperience { get; set; }

	[ForeignKey(nameof(City))]
	public int? CityID { get; set; }

	public City? City { get; set; }

	public string FullName => $"{FirstName} {LastName}";

	public virtual ICollection<Meeting>? Meetings { get; set; }
}