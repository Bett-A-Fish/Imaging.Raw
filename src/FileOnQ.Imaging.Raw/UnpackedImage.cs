﻿using System;
using System.Runtime.InteropServices;

namespace FileOnQ.Imaging.Raw
{
	/// <inheritdoc cref="IUnpackedImage"/>
	abstract unsafe class UnpackedImage : IUnpackedImage
	{
		/// <summary>
		/// Instantiates the default instance of
		/// <see cref="UnpackedImage"/>.
		/// </summary>
		/// <param name="libraw">
		/// The pointer to the LibRaw data structure.
		/// </param>
		internal UnpackedImage(IntPtr libraw) =>
			LibRaw = libraw;

		protected IntPtr LibRaw { get; set; }
		protected LibRaw.ProcessedImage* Image { get; set; }
		protected IImageProcessor Processor { get; set; }

		/// <inheritdoc cref="IUnpackedImage"/>
		public void Process(IImageProcessor imageProcessor)
		{
			if (Processor != null)
			{
				Processor.Dispose();
				Processor = null;
			}

			Processor = imageProcessor;
			var resetImage = Processor.Process(BuildRawImageData());

			if (resetImage && Image != null)
			{
				Raw.LibRaw.ClearMemory(Image);
				Image = null;
			}
		}

		/// <inheritdoc cref="IUnpackedImage"/>
		public void Write(string file) =>
			Processor.Write(BuildRawImageData(), file);

		/// <inheritdoc cref="IUnpackedImage"/>
		public ProcessedImage AsProcessedImage() =>
			Processor.AsProcessedImage(BuildRawImageData());

		/// <summary>
		/// Loads the current image into memory prior
		/// to processing.
		/// </summary>
		protected abstract void LoadImage();

		RawImageData BuildRawImageData()
		{
			if (Processor == null)
				throw new NullReferenceException("Image has not been processed, Call Process(IImageProcessor) first!");

			LoadImage();
			return new RawImageData
			{
				LibRawData = LibRaw,
				Bits = Image->Bits,
				Colors = Image->Colors,
				Height = Image->Height,
				Width = Image->Width,
				ImageType = (ImageType)Image->Type,
				Buffer = new Span<byte>((void*)GetBufferMemoryAddress(), (int)Image->DataSize)
			};
		}
		
		IntPtr GetBufferMemoryAddress()
		{
			// get the memory address of the data buffer.
			var memoryOffset = Marshal.OffsetOf(typeof(LibRaw.ProcessedImage), nameof(Raw.LibRaw.ProcessedImage.Data)).ToInt32();
			return (IntPtr)Image + memoryOffset;
		}


		~UnpackedImage() => Dispose(false);

		bool isDisposed;
		
		/// <inheritdoc cref="IDisposable"/>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (isDisposed)
				return;

			if (disposing)
			{
				// free managed resources
			}

			if (Image != null)
			{
				Raw.LibRaw.ClearMemory(Image);
				Image = (LibRaw.ProcessedImage*)IntPtr.Zero;
			}

			if (LibRaw == IntPtr.Zero)
			{
				// clear the pointer, but don't clear the memory. Let the pointer owner clear the memory
				LibRaw = IntPtr.Zero;
			}

			isDisposed = true;
		}
	}
}