using System;
using System.ComponentModel;

namespace CSXStatsViewer
{
    public class StatsEntry : INotifyPropertyChanged
    {
 
        #region Propreties
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        
        private string _unique;
        public string Unique
        {
            get { return _unique; }
            set
            {
                _unique = value;
                OnPropertyChanged("Unique");
            }
        }

        private uint _teamKills;
        public uint TeamKills
        {
            get { return _teamKills; }
            set
            {
                _teamKills = value;
                OnPropertyChanged("TeamKills");
            }
        }

        private uint _damage;
        public uint Damage
        {
            get { return _damage; }
            set
            {
                _damage = value;
                OnPropertyChanged("Damage");
            }
        }

        private uint _deaths;
        public uint Deaths
        {
            get { return _deaths; }
            set
            {
                _deaths = value;
                OnPropertyChanged("Deaths");
                OnPropertyChanged("NetScore");
            }
        }

        private Int32 _kills;
        public Int32 Kills
        {
            get { return _kills; }
            set
            {
                _kills = value;
                OnPropertyChanged("Kills");
                OnPropertyChanged("NetScore");
            }
        }

        private uint _shots;
        public uint Shots
        {
            get { return _shots; }
            set
            {
                _shots = value;
                OnPropertyChanged("Shots");
                OnPropertyChanged("Accuracy");
            }
        }

        private uint _hits;
        public uint Hits
        {
            get { return _hits; }
            set
            {
                _hits = value;
                OnPropertyChanged("Hits");
                OnPropertyChanged("Accuracy");
            }
        }

        private uint _headshots;
        public uint Headshots
        {
            get { return _headshots; }
            set
            {
                _headshots = value;
                OnPropertyChanged("Headshots");
            }
        }

        private uint _bDefusions;
        public uint BDefusions
        {
            get { return _bDefusions; }
            set
            {
                _bDefusions = value;
                OnPropertyChanged("BDefusions");
            }
        }

        private uint _bDefused;
        public uint BDefused
        {
            get { return _bDefused; }
            set
            {
                _bDefused = value;
                OnPropertyChanged("BDefused");
            }
        }

        private uint _bPlants;
        public uint BPlants
        {
            get { return _bPlants; }
            set
            {
                _bPlants = value;
                OnPropertyChanged("BPlants");
            }
        }

        private uint _bExplosions;
        public uint BExplosions
        {
            get { return _bExplosions; }
            set
            {
                _bExplosions = value;
                OnPropertyChanged("BExplosions");
            }
        }

        private uint[] _bodyHits;
        public uint[] BodyHits
        {
            get { return _bodyHits; }
            set
            {
                _bodyHits = value;
                OnPropertyChanged("BodyHits");
            }
        }

        public int NetScore { 
            get { return (int) (Kills - Deaths); }
        }

        public double Accuracy {
            get { return Math.Round((Hits/(float) Shots)*100,1); } 
        }

        #endregion

        public StatsEntry()
        {
            BodyHits = new uint[9];
        }

        public static StatsEntry operator +(StatsEntry a, StatsEntry b)
        {
            StatsEntry temp = new StatsEntry
                                  {
                                      Name = a.Name,
                                      Unique = a.Unique,
                                      Headshots = a.Headshots + b.Headshots,
                                      BDefused = a.BDefused + b.BDefused,
                                      BDefusions = a.BDefusions + b.BDefusions,
                                      BExplosions = a.BExplosions + b.BExplosions,
                                      BPlants = a.BPlants + b.BPlants,
                                      Damage = a.Damage + b.Damage,
                                      Deaths = a.Deaths + b.Deaths,
                                      Hits = a.Hits + b.Hits,
                                      Kills = a.Kills + b.Kills,
                                      Shots = a.Shots + b.Shots,
                                      TeamKills = a.TeamKills + b.TeamKills,
                                  };
            temp.BodyHits[0] = a.BodyHits[0] + b.BodyHits[0];
            temp.BodyHits[1] = a.BodyHits[1] + b.BodyHits[1];
            temp.BodyHits[2] = a.BodyHits[2] + b.BodyHits[2];
            temp.BodyHits[3] = a.BodyHits[3] + b.BodyHits[3];
            temp.BodyHits[4] = a.BodyHits[4] + b.BodyHits[4];
            temp.BodyHits[5] = a.BodyHits[5] + b.BodyHits[5];
            temp.BodyHits[6] = a.BodyHits[6] + b.BodyHits[6];
            temp.BodyHits[7] = a.BodyHits[7] + b.BodyHits[7];
            temp.BodyHits[8] = a.BodyHits[8] + b.BodyHits[8];

            return temp;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
