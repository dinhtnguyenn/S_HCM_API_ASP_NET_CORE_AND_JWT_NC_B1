using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ScoreModel
    {
        private string mssv;
        private int score;

        public ScoreModel(string mssv, int score)
        {
            this.mssv = mssv;
            this.score = score;
        }

        public string MSSV { get => mssv; set => mssv = value; }
        public int Score { get => score; set => score = value; }
    }
}