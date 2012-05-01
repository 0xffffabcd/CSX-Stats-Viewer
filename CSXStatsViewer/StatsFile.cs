using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CSXStatsViewer
{

    public static class StatsFile
    {
        /// <summary>
        /// Stats File version
        /// </summary>
        private const short RankVersion = 11;

        /// <summary>
        /// Saves the stats to a file
        /// </summary>
        /// <param name="entries">IEnumerable of StatsEntries</param>
        /// <param name="file">File where to save the stats to</param>
        /// <returns></returns>
        public static bool SaveEntriesToFile(IEnumerable<StatsEntry> entries, string file)
        {
            bool result = false;

            FileStream stream = File.Open(file, FileMode.Create);

            if (stream == null)
            {
                throw new FileLoadException();
            }

            BinaryWriter binaryWriter = new BinaryWriter(stream);

            try
            {
                binaryWriter.Write(RankVersion);


                foreach (var statsEntry in entries)
                {
                    binaryWriter.Write((UInt16)(Encoding.ASCII.GetBytes(statsEntry.Name).Length + 1));
                    binaryWriter.Write(Encoding.ASCII.GetBytes(statsEntry.Name));
                    binaryWriter.Write((byte)0);
                    binaryWriter.Write((UInt16)(Encoding.ASCII.GetBytes(statsEntry.Unique).Length + 1));
                    binaryWriter.Write(Encoding.ASCII.GetBytes(statsEntry.Unique));
                    binaryWriter.Write((byte)0);
                    
                    binaryWriter.Write(statsEntry.TeamKills);
                    binaryWriter.Write(statsEntry.Damage);
                    binaryWriter.Write(statsEntry.Deaths);
                    binaryWriter.Write(statsEntry.Kills);
                    binaryWriter.Write(statsEntry.Shots);
                    binaryWriter.Write(statsEntry.Hits);
                    binaryWriter.Write(statsEntry.Headshots);
                    binaryWriter.Write(statsEntry.BDefused);                    
                    binaryWriter.Write(statsEntry.BDefusions);
                    binaryWriter.Write(statsEntry.BPlants);
                    binaryWriter.Write(statsEntry.BExplosions);

                    for (int i = 0; i < statsEntry.BodyHits.Length; i++)
                    {
                        binaryWriter.Write(statsEntry.BodyHits[i]);
                    }
                }
                binaryWriter.Write(UInt16.MinValue);
                result = true;
            }
            catch
            {
                throw new FileLoadException("Error writing to the file");
            }
            finally
            {
                binaryWriter.Close();
                stream.Close();
            }
            return result;
        }

        /// <summary>
        /// Reads the stats entries
        /// </summary>
        /// <param name="file">CSX stats file</param>
        /// <returns></returns>
        public static ObservableCollection<StatsEntry> ReadEntriesToList(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException();
            }

            FileStream stream = File.Open(file, FileMode.Open);

            if (stream == null)
            {
                throw new FileLoadException();
            }

            BinaryReader br = new BinaryReader(stream);
            ObservableCollection<StatsEntry> list;

            try
            {
                short vers = br.ReadInt16();

                if (vers != RankVersion)
                {
                    throw new Exception("Bad stats version");
                }

                ushort num = br.ReadUInt16();
                list = new ObservableCollection<StatsEntry>();

                while (num != 0)
                {
                    StatsEntry entry = new StatsEntry();

                    byte[] name = br.ReadBytes(num);
                    num = br.ReadUInt16();
                    byte[] unique = br.ReadBytes(num);

                    entry.Name = Encoding.ASCII.GetString(name, 0, name.Length - 1);
                    entry.Unique = Encoding.ASCII.GetString(unique, 0, unique.Length - 1);

                    entry.TeamKills = br.ReadUInt32();
                    entry.Damage = br.ReadUInt32();
                    entry.Deaths = br.ReadUInt32();
                    entry.Kills = br.ReadInt32();
                    entry.Shots = br.ReadUInt32();
                    entry.Hits = br.ReadUInt32();
                    entry.Headshots = br.ReadUInt32();
                    entry.BDefusions = br.ReadUInt32();
                    entry.BDefused = br.ReadUInt32();
                    entry.BPlants = br.ReadUInt32();
                    entry.BExplosions = br.ReadUInt32();

                    for (int i = 0; i < entry.BodyHits.Length; i++)
                    {
                        entry.BodyHits[i] = br.ReadUInt32();
                    }

                    num = br.ReadUInt16();

                    list.Add(entry);
                }
            }
            catch
            {
                throw new FileLoadException("Error reading file");
            }
            finally
            {
                br.Close();
                stream.Close();
            }

            return list;
        }
    }
}