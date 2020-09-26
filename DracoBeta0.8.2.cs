
using CSR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Core.Tokens;
using YamlDotNet;
using System.Security.Cryptography;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using Newtonsoft.Json;
using System.Activities.Statements;
using System.Windows.Forms;

namespace Draco
{
    class DracoBeta
    {
        public class Playerconfig
        {
            public double Addition { get; set; }
            public string Name { get; set; }
            public string job { get; set; }
            public int level { get; set; }

        }

            public static void Dracoup(MCCSAPI api)
        {
            Dictionary<string, int> level = new Dictionary<string, int>();
            Dictionary<string, double> addition = new Dictionary<string, double>();
            Dictionary<string, string> uuid = new Dictionary<string, string>();
            Dictionary<string, string> xuid = new Dictionary<string, string>();
            Dictionary<string, string> job = new Dictionary<string, string>();
            Dictionary<string, string> ifban = new Dictionary<string, string>();
            string MoneyPath = "./plugins/Draco/RewardMoney.yaml";
            string playersPath = "./plugins/Draco/playersjob.yaml";
            string banPath = "./plugins/Draco/banplayers.yaml";
            #region //yaml转json示例
            //if (File.Exists("./plugins/Draco/config.yaml"))
            //{
            //    var r = new StringReader(File.ReadAllText("./plugins/Draco/config.yaml"));
            //    var deserializer = new DeserializerBuilder().Build();
            //    var yamlObject = deserializer.Deserialize(r);
            //    var serializer = new SerializerBuilder()
            //        .JsonCompatible()
            //        .Build();
            //    var json = serializer.Serialize(yamlObject);
            //    //Console.WriteLine(json);
            //    config rb = JsonConvert.DeserializeObject<config>(json);
            //    Console.WriteLine("是否开启攻击加成："+rb.ATNPlus);
            //}
            #endregion
            string awa = @"September 2020 Draco Release.
Thanks for using Draco.
本插件基于Mozilla协议开源
项目地址：
https://github.com/Sbaoor-fly/CSR-Draco
Mozilla协议具体信息：
http://www.mozilla.org/MPL/MPL-1.1.html
";
            if (File.Exists("./plugins/Draco/DracoBeta.txt"))
            {

            }
            else
            {
                DialogResult dr = MessageBox.Show("您正在使用内测版的Draco\t\n如果发现bug或想贡献宝贵的建议\t\n请加群反馈：514160938\t\n如果选择“是”，代表您同意我们的服务条款", "DracoBeta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //如果消息框返回值是Yes，显示新窗体
                if (dr == DialogResult.Yes)
                {
                    
                }
                //如果消息框返回值是No，关闭当前窗体
                else if (dr == DialogResult.No)
                {
                    //关闭当前窗体
                    var a = File.ReadAllLines("./awa,txt");
                }
                Directory.CreateDirectory("./plugins/Draco");
                File.AppendAllText(banPath, "#格式 玩家ID: 被ban原因", System.Text.Encoding.Default);
                File.AppendAllText(playersPath, "Steve: Miner", System.Text.Encoding.Default);
                File.AppendAllText(MoneyPath, "DeBug: false\ncow: 10", System.Text.Encoding.Default);
                File.AppendAllText("./plugins/Draco/DracoBeta.txt",awa, System.Text.Encoding.Default);
                Directory.CreateDirectory("./plugins/Draco/Miner");
                Directory.CreateDirectory("./plugins/Draco/Hunter");
                Directory.CreateDirectory("./plugins/Draco/Swordman");
                var t = Task.Run(async delegate
                {
                    await Task.Delay(10000);
                    api.runcmd("scoreboard objectives add money dummy");
                });
            }
            api.addBeforeActListener(EventKey.onLoadName, x =>
             {
                 var a = BaseEvent.getFrom(x) as LoadNameEvent;
                 uuid.Add(a.playername, a.uuid);
                 xuid.Add(a.playername, a.xuid);
                 YML banyml = new YML(banPath);
                 if (banyml.read(a.playername,"") != null) 
                 { ifban.Add(a.playername, "true"); }
                 else { ifban.Add(a.playername, "false"); }
                 YML plyml = new YML(playersPath);
                 if (plyml.read(a.playername, "") != null)
                 {
                     if (File.Exists("./plugins/Draco/" + plyml.read(a.playername, "") + "/" + xuid[a.playername] + ".json"))
                     {
                         var jsontext = File.ReadAllText("./plugins/Draco/" + plyml.read(a.playername, "") + "/" + xuid[a.playername] + ".json");
                         Playerconfig rb = JsonConvert.DeserializeObject<Playerconfig>(jsontext);
                         addition.Add(a.playername, rb.Addition);
                         job.Add(a.playername, rb.job);
                         level.Add(a.playername, rb.level);
                         Console.WriteLine("[Draco]配置文件读取成功！");
                     }
                     else
                     {
                         Playerconfig write = new Playerconfig
                         {
                             Addition = 0.1,
                             Name = a.playername,
                             level = 1,
                             job = plyml.read(a.playername, "")
                         };
                         string str = JsonConvert.SerializeObject(write);
                         File.WriteAllText("./plugins/Draco/" + plyml.read(a.playername, "") + "/" + xuid[a.playername] + ".json", str, System.Text.Encoding.Default);
                         level.Add(a.playername, 1);
                         addition.Add(a.playername, 0.1);
                         job.Add(a.playername, plyml.read(a.playername, ""));
                         Console.WriteLine("[Draco]配置文件创建成功！");
                     }
                 }
                 else
                 {
                     job.Add(a.playername, "无职业");
                     level.Add(a.playername, 0);
                     Console.WriteLine("[Draco]{0}进入了游戏", a.playername);
                 }
                 return true;
             });
            api.addBeforeActListener(EventKey.onRespawn, x =>
             {
                 YML banyml2 = new YML(banPath);
                 var a = BaseEvent.getFrom(x) as RespawnEvent;
                 if(ifban[a.playername] == "true")
                 {
                     api.runcmd("kick " + a.playername + " " + banyml2.read(a.playername, ""));
                 }
                 else
                 {
                     api.setPlayerSidebar(uuid[a.playername], "个人信息", "[\"玩家名称：" + a.playername + "\",\"职业：" + job[a.playername] + "\"]");
                 }
                 return true;
             });
            api.addBeforeActListener(EventKey.onPlayerLeft, x =>
            {
                var a = BaseEvent.getFrom(x) as PlayerLeftEvent;
                level.Remove(a.playername);
                addition.Remove(a.playername);
                uuid.Remove(a.playername);
                xuid.Remove(a.playername);
                job.Remove(a.playername);
                ifban.Remove(a.playername);
                Console.WriteLine("[Draco]{0}离开了游戏，正在保存数据", a.playername);
                return true;
            });
            api.addBeforeActListener(EventKey.onAttack, x =>
             {
                 var a = BaseEvent.getFrom(x) as AttackEvent;
                 if (job[a.playername] == "Swordman")
                 {
                     Random r = new Random();
                     if (r.Next(100) < level[a.playername]*5)
                     {
                         try
                         {
                             if (Convert.ToInt32(level[a.playername] * addition[a.playername]) != 0)
                             {
                                 api.runcmd("effect " + a.playername + " strength " + Convert.ToInt32(level[a.playername] * addition[a.playername]) + " 2 true");
                             }
                         }
                         catch { Console.WriteLine("WARNING!!!"); }
                     }
                 }
                 return true;
             });
            api.addBeforeActListener(EventKey.onDestroyBlock, x =>
             {
                 var a = BaseEvent.getFrom(x) as DestroyBlockEvent;
                 if (job[a.playername] == "Miner")
                 {
                     Random r = new Random();
                     if (r.Next(100) < level[a.playername]*5)
                     {
                         try
                         {                         
                             if (Convert.ToInt32(level[a.playername] * addition[a.playername]) != 0)
                             {
                                 api.runcmd("effect " + a.playername + " haste " + Convert.ToInt32(level[a.playername] * addition[a.playername]) + " 2 true");
                             }
                         }
                         catch { Console.WriteLine("WARNING!!!"); }
                     }
                 }
                 return true;
             });
            api.addBeforeActListener(EventKey.onMobDie, x =>
             {
             var a = BaseEvent.getFrom(x) as MobDieEvent;
             string str = a.mobtype;
             string[] sArray = str.Split(new char[2] { '.', '.' });
                 YML yml = new YML(MoneyPath);
                 if (yml.read("DeBug", "") == "true") { Console.WriteLine(sArray[1]); }
                 if (yml.read(sArray[1], "") != null & a.srcname != "")
                 {
                     api.runcmd("scoreboard players add " + a.srcname + " money " + yml.read(sArray[1], ""));
                     api.runcmd("tellraw \"" + a.srcname + "\" {\"rawtext\":[{\"text\":\"§6击杀奖励" + yml.read(sArray[1], "") + "金币！\"}]}");
                     if (job[a.srcname] == "Hunter")
                     {
                         //Console.WriteLine(Convert.ToInt32(yml.read(sArray[1], "")) / 2);
                         int b = Convert.ToInt32(yml.read(sArray[1], "")) / 2;
                         if(b != 0)
                         {
                             api.runcmd("scoreboard players add " + a.srcname + " money " + b);
                             api.runcmd("tellraw \"" + a.srcname + "\" {\"rawtext\":[{\"text\":\"[§3猎手加成§r]§6 额外奖励" + b + "金币！\"}]}");
                         }
                         

                     }
                 }

                 return true;
             });
            api.addBeforeActListener(EventKey.onInputText, x =>
            {
                var a = BaseEvent.getFrom(x) as InputTextEvent;
                string[] diam = { "§2主世界", "§4地狱", "§e末地" };
                string[] diam2 = { "主世界", "地狱", "末地" };
                if (a.msg == "here")
                {
                    api.runcmd("tellraw @a {\"rawtext\":[{\"text\":\"[" + diam[a.dimensionid] + "§r] §3" + a.playername + "§r 共享了一个坐标 >> " + Convert.ToInt32(a.XYZ.x) +" "+Convert.ToInt32(a.XYZ.y)+" "+Convert.ToInt32(a.XYZ.z) +"\"}]}");
                    Console.WriteLine("!{0} 共享了坐标:位于{4}的 {1}，{2}，{3} ", a.playername, Convert.ToInt32(a.XYZ.x), Convert.ToInt32(a.XYZ.y), Convert.ToInt32(a.XYZ.z), diam2[a.dimensionid]);
                    return false;
                }
                if(a.msg == "levelup")
                {
                    try
                    {
                        int b = api.getscoreboard(uuid[a.playername], "money");
                        int c = level[a.playername] * 100;
                        if (b >= c)
                        {
                            Playerconfig write = new Playerconfig();
                            write.Addition = addition[a.playername];
                            write.Name = a.playername;
                            write.level = level[a.playername] + 1;
                            write.job = job[a.playername];
                            string str = JsonConvert.SerializeObject(write);
                            File.WriteAllText("./plugins/Draco/" + job[a.playername] + "/" + xuid[a.playername] + ".json", str, System.Text.Encoding.Default);
                            level[a.playername] += 1;
                            api.runcmd("scoreboard players remove " + a.playername + " money " + c);
                            api.runcmd("tellraw \"" + a.playername + "\" {\"rawtext\":[{\"text\":\"§b成功升到了" + level[a.playername] + "级！花费了" + c + "金币！\"}]}");
                        }
                        else
                        {
                            api.runcmd("tellraw \"" + a.playername + "\" {\"rawtext\":[{\"text\":\"§4金币不足！距离" + (level[a.playername] + 1) + "级还需要" + c + "金币！\"}]}");
                            api.runcmd("tellraw \"" + a.playername + "\" {\"rawtext\":[{\"text\":\"§4当前金币：" + b + "\"}]}");
                        }
                    }
                    catch
                    {
                        api.runcmd("tellraw \"" + a.playername + "\" {\"rawtext\":[{\"text\":\"§4升级失败！\"}]}");
                    }

                }
                else
                {
                    api.runcmd("tellraw @a {\"rawtext\":[{\"text\":\"[" + diam[a.dimensionid] + "§r][§3"+level[a.playername]+"级" + job[a.playername] + "§r " + a.playername + "] >> " + a.msg + "\"}]}");
                    Console.WriteLine("![{2} {0}] >> {1}", a.playername, a.msg,job[a.playername]);
                }
                return false;               
            });
        }
    }
}
namespace CSR
{
    partial class Plugin
    {

        public static void onStart(MCCSAPI api)
        {
            // TODO 此接口为必要实现
            Draco.DracoBeta.Dracoup(api);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[Draco]加载成功！！");
            Console.WriteLine("[Draco]您正在使用DracoBeta0.2.2");
            Console.WriteLine(@"    ____                      
   / __ \_________ __________ 
  / / / / ___/ __ `/ ___/ __ \
 / /_/ / /  / /_/ / /__/ /_/ /
/_____/_/   \__,_/\___/\____/ ");
            Console.WriteLine("[Draco]Bug反馈请加QQ：2023786106");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}