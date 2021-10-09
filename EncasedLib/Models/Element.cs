namespace EncasedLib.Models
{
    using System;

	public class Element
	{
		public Language Lng { get; set; }
		public Byte[] Address { get; set; }
		public Byte Xa { get; set; }
		public UInt32 Length { get; set; }
		public Byte[] Data { get; set; }
		public String DataTxt { get; set; }
		public Byte[] Complete { get; set; }

		public Element(Language lng, Byte[] complete)
        {
			Lng = lng;
			Complete = complete;



		}

        public override String ToString()
        {
            return base.ToString();
        }
    }
}