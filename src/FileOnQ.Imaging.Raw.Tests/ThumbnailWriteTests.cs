﻿using System;
using System.IO;
using NUnit.Framework;

namespace FileOnQ.Imaging.Raw.Tests
{
	[TestFixture("images/sample1.cr2")]
	[TestFixture("images/@signatureeditsco(1).dng")]
	[TestFixture("images/@signatureeditsco.dng")]
	[TestFixture("images/canon_eos_r_01.cr3")]
	[TestFixture("images/Christian - .unique.depth.dng")] // this file might have a bitmap instead of a jpeg
	[TestFixture("images/DSC_0118.nef")]
	[TestFixture("images/DSC02783.ARW")]
	[TestFixture("images/PANA2417.RW2")]
	[TestFixture("images/PANA8392.RW2")]
	[TestFixture("images/photo by @Dupe.png--@Emily.rosegold.arw")]
	[TestFixture("images/signature edits APC_00171.dng")]
	[TestFixture("images/signature edits free raws P1015526.dng")]
	[TestFixture("images/signature edits free raws_DSC7082.NEF")]
	[TestFixture("images/signatureeditsfreerawphoto.NEF")]
	public class ThumbnailWriteTests
	{
		readonly string input;
		readonly string output;
		readonly string expectedThumbnail;

		public ThumbnailWriteTests(string path)
		{
			input = path;
			
			var filename = Path.GetFileNameWithoutExtension(input);
			output = $"{filename}.thumb.jpeg";

			var directory = Path.GetDirectoryName(input) ?? string.Empty;
			expectedThumbnail = Path.Combine(directory, $"{filename}.thumb.jpeg");
		}

		[OneTimeSetUp]
		public void Execute()
		{
			using (var image = new RawImage(input))
			{
				var thumbnail = image.UnpackThumbnail();
				thumbnail.Write(output);
			}
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			if (File.Exists(output))
				File.Delete(output);
		}
		
		[Test]
		public void ThumbnailWrite_FileExists_Test() =>
			Assert.IsTrue(File.Exists(output));

		[Test]
		public void ThumbnailWrite_MatchJpeg_Test()
		{
			var expectedBuffer = new Span<byte>(File.ReadAllBytes(expectedThumbnail));
			var actualBuffer = new Span<byte>(File.ReadAllBytes(output));

			Assert.IsTrue(actualBuffer.Length > 0);
			Assert.AreEqual(expectedBuffer.Length, actualBuffer.Length);

			// This is a slow operation, there may be span specific APIs to speed this up
			for (int index = 0; index < expectedBuffer.Length; index++)
				Assert.AreEqual(expectedBuffer[index], actualBuffer[index]);
		}
	}
}