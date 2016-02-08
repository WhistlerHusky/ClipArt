using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ClipboardHookinkg
{
    /// <summary>
    /// 
    /// </summary>
    public class ClipboardEntity : INotifyCollectionChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ClipboardFormat ClipboardFormat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iconImage"></param>
        /// <param name="newData"></param>
        /// <param name="newClipboardFormat"></param>
        /// <param name="newDataTime"></param>
        public ClipboardEntity(byte[] iconImage, object newData, ClipboardFormat newClipboardFormat, string newDataTime)
        {
            this.Image = iconImage;
            this.Data = newData;
            this.ClipboardFormat = newClipboardFormat;
            this.DateTime = newDataTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("DataType = {0}, DateTime = {1}", this.ClipboardFormat, this.DateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
