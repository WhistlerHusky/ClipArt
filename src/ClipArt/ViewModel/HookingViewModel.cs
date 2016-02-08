using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using ClipboardHookinkg;

namespace ClipArt.ViewModel
{
    public class ClipboardWindowViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly List<ClipboardEntity> _clipboardEntity;

        /// <summary>
        /// 
        /// </summary>
        public ICollectionView ClipboardEntityView
        {
            get { return CollectionViewSource.GetDefaultView(_clipboardEntity); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clipboardEntitie"></param>
        public ClipboardWindowViewModel(List<ClipboardEntity> clipboardEntitie)
        {
            _clipboardEntity = clipboardEntitie;
        }
    }
}
