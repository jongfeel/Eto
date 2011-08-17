using System;
using SD = System.Drawing;
using Eto.Forms;
using Eto.Drawing;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace Eto.Platform.Mac
{
	
	
	public class DockLayoutHandler : MacLayout<NSView, DockLayout>, IDockLayout
	{
		Control child;
		Padding padding;
		
		public override NSView Control {
			get {
				return (NSView)Widget.Container.ContainerObject;
			}
			protected set {
				base.Control = value;
			}
		}
		
		public Eto.Drawing.Padding Padding {
			get { return padding; }
			set {
				padding = value;
				SetChildFrame();
			}
		}
		
		public override void SizeToFit ()
		{
			/*var container = child as Container;
			if (container != null)
			{
				var layout = container.Layout as IMacLayout;
				if (layout != null) layout.SizeToFit ();
				var size = ((NSView)container.ControlObject).Frame;
				SetContainerSize (size.Size);
			}
			else*/ if (child != null)
			{
				AutoSize (child);
				var c = child.ControlObject as NSView;
				if (c != null) {
					SetContainerSize (c.Frame.Size);
				}
			}
		}
		
		void SetChildFrame()
		{
			if (child == null) return;
			
			NSView parent = (NSView)ControlObject;
			
			NSView childControl = (NSView)child.ControlObject;
			var frame = parent.Frame;
			
			if (frame.Width > padding.Horizontal && frame.Height > padding.Vertical)
			{
				frame.X = padding.Left;
				frame.Width -= padding.Horizontal;
				frame.Y = padding.Top;
				frame.Height -= padding.Vertical;
			}
			
			childControl.Frame = frame;
		}
				
		public void Add(Control child)
		{
			if (this.child != null) { 
				((NSView)this.child.ControlObject).RemoveFromSuperview(); this.child = null; 
			}
			if (child != null)
			{
				NSView childControl = (NSView)child.ControlObject;
				childControl.AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;
				this.child = child;
				SetChildFrame();
				NSView parent = (NSView)ControlObject;
				parent.AddSubview(childControl);
			}
		}

		public void Remove(Control child)
		{
			NSView childControl = (NSView)child.ControlObject;
			childControl.RemoveFromSuperview();
			this.child = null;
		}

		public override void SetContainerSize (SD.SizeF size)
		{
			base.SetContainerSize (size);
			SetChildFrame();
		}
	}
}
