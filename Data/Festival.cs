using StardewModdingAPI.Utilities;
using SVHeadlessHost.Enums;
using SVHeadlessHost.Handler;

namespace SVHeadlessHost.Data
{
    public class Festival
    {
        public string Name { get; }

        public ushort Day { get; }

        public int Year { get; }

        public Season Season { get; }

        public SDate Date { get; }

        public IHandler FestivalHandler { get; }

        public Festival(string name, int day, Season season, IHandler festivalHandler, int year = 0)
            : this(name, (ushort)day, season, festivalHandler, year)
        {
        }

        public Festival(string name, ushort day, Season season, IHandler festivalHandler, int year = 0)
        {
            this.Name = name;
            this.Day = day;
            this.Season = season;
            this.FestivalHandler = festivalHandler;

            if (year > 0)
            {
                this.Date = new SDate(day, season.ToString().ToLower());
            }
            else
            {
                this.Date = new SDate(day, season.ToString().ToLower(), year);
            }
        }
    }
}
