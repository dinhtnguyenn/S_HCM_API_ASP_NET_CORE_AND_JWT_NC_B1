using System;
namespace WebApplication2.Models
{
	public class fpt_LeaderBoard
	{
        public fpt_LeaderBoard(string username, int? score)
        {
            this.username = username;
            this.score = score;
        }

        public string username { get; set; }
		public int? score { get; set; }
	}
}

