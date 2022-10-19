using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace ZFrameWork
{
    public class ResTotalVersion
    {
        [SerializeField] private int versionCode;
        public int VersionCode { get { return versionCode; } }

        private string savePath = "";
        public ResTotalVersion() { }
        public string SavePath { get {
                if (!string.IsNullOrEmpty(savePath))
                    return savePath;
                else
                    return savePath;
            } }
        public ResTotalVersion(string savePth) {
            this.savePath = savePth;
        }
        public void Save() {
            this.versionCode++;
            this.ToJson().SaveTo(SavePath);
        }
        public void LoadByText(string text) {

            text.FromJsonOverwrite(this);
            this.ToJson().SaveTo(SavePath);
        }
        public void Load() {
            var text = FileHelper.FileReadAllText(SavePath);

            if (string.IsNullOrEmpty(text))
                this.Save();
            else
                text.FromJsonOverwrite(this);
        }
    }
}
