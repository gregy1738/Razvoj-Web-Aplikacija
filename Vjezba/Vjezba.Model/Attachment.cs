using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
	public class Attachment
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public DateTime UploadDate { get; set; }

		[ForeignKey(nameof(Client))]
		public int ClientID { get; set; }
		public Client Client { get; set; }
	}
}
