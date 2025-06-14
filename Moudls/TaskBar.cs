﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1.Moudls
{
  public  class TaskBar:BindableBase
    {
		private string  icon;

		public string Icon
        {
			get { return icon; }
			set { icon = value; }
		}

		private string title;

		public string Title
        {
			get { return title; }
			set { title = value; }
		}

		private string content;

		public string Content
        {
			get { return content; }
			set { content = value; }
		}


		private Brush color;

		public Brush Color
        {
			get { return color; }
			set { color = value; }
		}

		private string target;

		public string Target
        {
			get { return target; }
			set { target = value; }
		}



	}
}
