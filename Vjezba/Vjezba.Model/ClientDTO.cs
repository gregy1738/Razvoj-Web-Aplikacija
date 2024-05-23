using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vjezba.Model;

public class ClientDTO
{
	public int ID { get; set; }
	public string FullName { get; set; }
	public string Address { get; set; }
	public CityDTO City { get; set; }
	public string Email { get; set; }

}