using System;
using System.Runtime.InteropServices;

namespace Embprpusr.Managed
{
    [StructLayout(LayoutKind.Sequential)]
    struct SpiControllerConfig
    {
        public uint connectionSpeed;
        public ushort dataBitLength;
        public ushort spiMode;
    }

    enum SpiTransferFlag
    {
        None = 0,
        TransferFlagSequential = 1
    }

    static class EmbprpusrInterop
    {
        private const string EmbprpusrDll = "embprpusr.dll";

        //
        // ADC - Analog to Digital Converters
        //
        [DllImport(EmbprpusrDll, EntryPoint = "AdcCreateInstance", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int AdcCreateInstance(uint converterIndex, out IntPtr adcPtr);

        [DllImport(EmbprpusrDll, EntryPoint = "AdcFree", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, SetLastError = true)]
        internal static extern void AdcFree(IntPtr adcInst);

        [DllImport(EmbprpusrDll, EntryPoint = "AdcSampleChannel", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int AdcSampleChannel(IntPtr adcInst, uint channelNum);

        //
        // GPIO
        //

        // single pin operations
        [DllImport(EmbprpusrDll, EntryPoint = "GpioRead", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int GpioRead(uint pniNumber, ref int value);

        // 0 = input, 1 = output
        [DllImport(EmbprpusrDll, EntryPoint = "GpioSetDir", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int GpioSetDir(uint pinNumber, uint direction);

        [DllImport(EmbprpusrDll, EntryPoint = "GpioWrite", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int GpioWrite(uint pinNumber, uint direction);

        //
        // I2C_CONTROLLER
        // 
        [DllImport(EmbprpusrDll, EntryPoint = "I2CCreateInstance", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int I2CCreateInstance(uint controllerIndex, uint slaveAddress, uint connctionSpeed, out IntPtr i2cPtr);

        [DllImport(EmbprpusrDll, EntryPoint = "I2CFree", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, SetLastError = true)]
        internal static extern void I2CFree(IntPtr i2cInstPtr);

        //wtf, this method does not exist in embprpusr.dll
        [DllImport(EmbprpusrDll, EntryPoint = "I2CLockController", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int I2CLockController(IntPtr i2cInstPtr);

        [DllImport(EmbprpusrDll, EntryPoint = "I2CRead", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int I2CRead(IntPtr i2cInstPtr, IntPtr receiveBuffer, uint recieveBufferSize, ref uint bytesReturned);

        //wtf, this method does not exist in embprpusr.dll
        [DllImport(EmbprpusrDll, EntryPoint = "I2CUnlockController", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, SetLastError = true)]
        internal static extern void I2CUnlockController(IntPtr i2cInstPtr);

        [DllImport(EmbprpusrDll, EntryPoint = "I2CWrite", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int I2CWrite(IntPtr i2cInstPtr, IntPtr sendBuffer, uint sendBufferSize, ref uint bytesWritten);

        // Does a write, then a read without sending a stop bit between the write and read
        [DllImport(EmbprpusrDll, EntryPoint = "I2CWriteReadAtomic", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int I2CWriteReadAtomic(IntPtr i2cInstPtr, IntPtr sendBuffer, uint sendBufferSize, IntPtr receiveBuffer, uint receiveBufferSize, ref uint bytesReturned);

        //
        // PWM - Pulse Width Modulation
        //

        //
        // Updates the duty cycle for a pin on which PWM
        // has already been started.
        //
        // DutyCycle - 0 corresponds to 0% duty cycle
        //             PWM_MAX_DUTYCYCLE corresponds to 100%
        //             duty cycle
        //
        // Returns an error if PWM is not started on this pin
        //
        [DllImport(EmbprpusrDll, EntryPoint = "PwmSetDutyCycle", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int PwmSetDutyCycle(uint pinNumber, uint dutyCycle);

        //
        // Start PWM on the specified pin. 
        //
        // The hardware will attempt to match the requested settings
        // as closely as possible. Other GPIO operations cannot be
        // performed while PWM is started on a pin
        //
        // FrequencyInHertz - the requested frequency of the PWM signal,
        //                    must be greater than 0
        //
        // DutyCycle - the duty cycle as a proportion from 0 to
        //             PWM_MAX_DUTYCYCLE
        //
        [DllImport(EmbprpusrDll, EntryPoint = "PwmStart", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int PwmStart(uint pinNumber, uint frequencyInHertz, uint dutyCycle);

        //
        // Stops PWM on the specified pin. The pin will be an OUTPUT
        // held LOW after the call to PwmStop.
        //
        [DllImport(EmbprpusrDll, EntryPoint = "PwmStop", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, SetLastError = true)]
        internal static extern void PwmStop(uint pinNumber);
        
        //
        // SPI_CONTROLLER
        //
        [DllImport(EmbprpusrDll, EntryPoint = "SpiCreateInstance", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int SpiCreateInstance(uint controllerIndex, out IntPtr spiPtr);

        [DllImport(EmbprpusrDll, EntryPoint = "SpiFree", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, SetLastError = true)]
        internal static extern void SpiFree(IntPtr spiPtr);

        [DllImport(EmbprpusrDll, EntryPoint = "SpiGetControllerConfig", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int SpiGetControllerConfig(IntPtr spiInst, ref SpiControllerConfig config);

        [DllImport(EmbprpusrDll, EntryPoint = "SpiSetControllerConfig", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int SpiSetControllerConfig(IntPtr spiInst, ref SpiControllerConfig config);

        [DllImport(EmbprpusrDll, EntryPoint = "SpiTransfer", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false, PreserveSig = true, SetLastError = true)]
        internal static extern int SpiTransfer(IntPtr spiInst, SpiTransferFlag flags, IntPtr sendBuffer, uint sendBufferSize, IntPtr recieveBuffer, uint receiveBufferSize);
    }
}
