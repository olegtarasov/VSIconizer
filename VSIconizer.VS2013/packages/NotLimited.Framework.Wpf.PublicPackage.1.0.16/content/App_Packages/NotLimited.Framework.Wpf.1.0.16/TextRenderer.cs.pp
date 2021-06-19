//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Windows;
using System.Windows.Media;

namespace NotLimited.Framework.Wpf
{
	public class TextRenderer
	{
		private Typeface _typeface;
		private GlyphTypeface _glyphFace;
		private double _fontSize;
		private double _fontHeight;

		public TextRenderer()
			: this(new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal))
		{
		}

		public TextRenderer(Typeface typeface)
		{
			_typeface = typeface;
			_fontSize = 12;
			CreateGlyphFace();
		}

		public Typeface Typeface
		{
			get { return _typeface; }
			set { _typeface = value; CreateGlyphFace(); }
		}

		public double FontSize
		{
			get { return _fontSize; }
			set { _fontSize = value; RecalculateFontHeight(); }
		}

		public double FontHeight { get { return _fontHeight; } }

		private void CreateGlyphFace()
		{
			if (!_typeface.TryGetGlyphTypeface(out _glyphFace))
				throw new InvalidOperationException("Can't get the plygh typeface!");
			RecalculateFontHeight();
		}

		private void RecalculateFontHeight()
		{
			_fontHeight = _glyphFace.Height * FontSize;
		}

		public Size MeasureText(string text)
		{
			double width = 0;
			ushort idx;

			for (int i = 0; i < text.Length; i++)
			{
				idx = _glyphFace.CharacterToGlyphMap[text[i]];
				width += _glyphFace.AdvanceWidths[idx] * _fontSize;
			}

			return new Size(width, _fontHeight);
		}

		public void DrawText(DrawingContext dc, string text, Point origin)
		{
			var indexes = new ushort[text.Length];
			var widths = new double[text.Length];

			for (int i = 0; i < text.Length; i++)
			{
				indexes[i] = _glyphFace.CharacterToGlyphMap[text[i]];
				widths[i] = _glyphFace.AdvanceWidths[indexes[i]] * _fontSize;
			}

			var run = new GlyphRun(_glyphFace, 0, false, _fontSize, indexes, origin, widths, null, null, null, null, null, null);
			dc.DrawGlyphRun(Brushes.Black, run);
		}
	}
}