using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RogueLikePlatfromer
{
    static class LevelIO
    {
        static string path = Setup.ContentDevice.RootDirectory + "/Levels/";

        public static void Save()
        {

            if (Directory.Exists(path))
            {
                Write();
            }
            else
            {
                Directory.CreateDirectory(path);
                Write();
            }
        }

        public static void Load()
        {

            if (Directory.Exists(path))
            {
                Read();
            }
            else
            {
                MessageBox.Show("The file or folder does not exist!", "Important Message");
            }
        }

        private static void Write()
        {
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
            using (XmlWriter writer = XmlWriter.Create(path + "map.xml", settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("level");

                writer.WriteStartElement("LevelObjects");

                foreach (var o in Scene.levelObjects)
                {
                    writer.WriteStartElement("tile");

                    writer.WriteAttributeString("type", o.tileType.ToString());
                    writer.WriteAttributeString("x", o.position.X.ToString());
                    writer.WriteAttributeString("y", o.position.Y.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                writer.WriteStartElement("Actor");

                foreach (var o in Scene.ActorObjects)
                {
                    writer.WriteStartElement("actor");

                    writer.WriteAttributeString("x", o.position.X.ToString());
                    writer.WriteAttributeString("y", o.position.Y.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();


                writer.WriteStartElement("Player");

                foreach (var o in Scene.PlayerList)
                {
                    writer.WriteStartElement("player");

                    writer.WriteAttributeString("x", o.position.X.ToString());
                    writer.WriteAttributeString("y", o.position.Y.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();


                writer.WriteEndDocument();
            }
        }

        private static void Read()
        {
            XDocument xdoc = XDocument.Load(path + "map.xml");

            var lvl = xdoc.Descendants("level");

            foreach (var o in lvl)
            {
                foreach (var p in o.Descendants("LevelObjects"))
                {
                    foreach (var t in p.Descendants("tile"))
                    {
                        //result.AppendLine("LO " + t.Attribute("x").Value);
                        Scene.levelObjects.Add(new Tile((TileType)Enum.Parse(typeof(TileType), t.Attribute("type").Value), new Vector2(float.Parse(t.Attribute("x").Value), float.Parse(t.Attribute("y").Value))));

                    }
                }

                foreach (var p in o.Descendants("Actor"))
                {
                    foreach (var t in p.Descendants("actor"))
                    {
                        //result.AppendLine("OB " + t.Attribute("x").Value);
                        Scene.ActorObjects.Add(new Enemy(new Vector2(float.Parse(t.Attribute("x").Value), float.Parse(t.Attribute("y").Value))));

                    }
                }

                foreach (var p in o.Descendants("Player"))
                {
                    foreach (var t in p.Descendants("player"))
                    {
                        //result.AppendLine("OB " + t.Attribute("x").Value);
                        Scene.PlayerList.Add(new Player(new Vector2(float.Parse(t.Attribute("x").Value), float.Parse(t.Attribute("y").Value))));

                    }
                }
            }
        }
    }
}
