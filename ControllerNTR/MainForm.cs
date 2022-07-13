using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.DualShock4;
using Nefarius.ViGEm.Client.Exceptions;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace ControllerNTR
{
    public partial class MainForm : Form
    {
        ViGEmClient viGEmClient = null;
        Controller[] realControllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };
        IDualShock4Controller[] virtualDualShock4Controllers = new IDualShock4Controller[] { null, null, null, null };
        IXbox360Controller[] virtualXbox360Controllers = new IXbox360Controller[] { null, null, null, null };
        Stopwatch controllerReportStopWatch = new Stopwatch();
        CancellationTokenSource cancellationSource;
        const int reportDelayMs = 4;

        public MainForm()
        {
            InitializeComponent();
            try
            {
                viGEmClient = new ViGEmClient();
            }
            catch (VigemAlreadyConnectedException ex)
            {
                MessageBox.Show("ViGEm 驱动程序正在被其他程序占用");
            }
            catch (VigemBusNotFoundException ex)
            {
                MessageBox.Show("未安装 ViGEm 驱动程序");
            }
            catch (VigemBusAccessFailedException ex)
            {
                MessageBox.Show("ViGEm 驱动程序拒绝访问");
            }
            catch (VigemBusVersionMismatchException ex)
            {
                MessageBox.Show("ViGEm 驱动程序版本与程序不匹配");
            }
            catch (Exception ex)
            {
                MessageBox.Show("未知错误：" + ex.Message);
            }
        }

        private enum GamepadType
        {
            DualShock4,
            Xbox360
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.Font = SystemFonts.MessageBoxFont;
        }

        private async Task GamepadChecker(GamepadType gamepadType, byte controllersBitMask, CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                controllerReportStopWatch.Restart();

                foreach (var selectRealControler in realControllers)
                {
                    int index = (int)selectRealControler.UserIndex;
                    int bitSet = 0b1 << index;
                    if ((bitSet & controllersBitMask) == 0) 
                    {
                        // 跳过手柄
                        continue;
                    }
                    if (selectRealControler.IsConnected)
                    {
                        UpdateSwitchVirtualController(gamepadType, index, true);
                        var state = selectRealControler.GetState();
                        if (gamepadType == GamepadType.DualShock4)
                        {
                            var virtualDualShock4Controller = virtualDualShock4Controllers[index];
                            if (virtualDualShock4Controller != null)
                            {
                                reportVirtualDualShock4Controller(virtualDualShock4Controller, state);
                            }
                        }
                        else
                        {
                            var virtualXbox360Controller = virtualXbox360Controllers[index];
                            if (virtualXbox360Controller != null)
                            {
                                reportVirtualXbox360Controller(virtualXbox360Controller, state);
                            }
                        }
                    }
                    else
                    {
                        UpdateSwitchVirtualController(gamepadType, index, false);
                    }
                }

                controllerReportStopWatch.Stop();
                var runingTimeMs = controllerReportStopWatch.ElapsedMilliseconds;
                var finalDelayTimeMs = reportDelayMs - runingTimeMs;
                if (finalDelayTimeMs > 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(finalDelayTimeMs));
                }
                await Task.Delay(TimeSpan.FromMilliseconds(reportDelayMs));
            }
            if (gamepadType == GamepadType.DualShock4)
            {
                for (int i = 0; virtualDualShock4Controllers.Length > i; i++)
                {
                    if (virtualDualShock4Controllers[i] != null)
                    {
                        virtualDualShock4Controllers[i].Disconnect();
                        virtualDualShock4Controllers[i] = null;
                    }
                }
            }
            else
            {
                for (int i = 0; virtualXbox360Controllers.Length > i; i++)
                {
                    if (virtualXbox360Controllers[i] != null)
                    {
                        virtualXbox360Controllers[i].Disconnect();
                        virtualXbox360Controllers[i] = null;
                    }
                }
            }
        }

        private void reportVirtualXbox360Controller(IXbox360Controller virtualXbox360Controller, State state)
        {
            var wButtons = state.Gamepad.Buttons;

            virtualXbox360Controller.SetButtonState(Xbox360Button.Up, (wButtons & GamepadButtonFlags.DPadUp) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.Down, (wButtons & GamepadButtonFlags.DPadDown) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.Left, (wButtons & GamepadButtonFlags.DPadLeft) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.Right, (wButtons & GamepadButtonFlags.DPadRight) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.Start, (wButtons & GamepadButtonFlags.Start) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.Back, (wButtons & GamepadButtonFlags.Back) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.LeftThumb, (wButtons & GamepadButtonFlags.LeftThumb) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.RightThumb, (wButtons & GamepadButtonFlags.RightThumb) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.LeftShoulder, (wButtons & GamepadButtonFlags.LeftShoulder) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.RightShoulder, (wButtons & GamepadButtonFlags.RightShoulder) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.A, (wButtons & GamepadButtonFlags.A) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.B, (wButtons & GamepadButtonFlags.B) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.X, (wButtons & GamepadButtonFlags.X) != 0);
            virtualXbox360Controller.SetButtonState(Xbox360Button.Y, (wButtons & GamepadButtonFlags.Y) != 0);
            virtualXbox360Controller.SetSliderValue(Xbox360Slider.LeftTrigger, state.Gamepad.LeftTrigger);
            virtualXbox360Controller.SetSliderValue(Xbox360Slider.RightTrigger, state.Gamepad.RightTrigger);
            virtualXbox360Controller.SetAxisValue(Xbox360Axis.LeftThumbX, state.Gamepad.LeftThumbX);
            virtualXbox360Controller.SetAxisValue(Xbox360Axis.LeftThumbY, state.Gamepad.LeftThumbY);
            virtualXbox360Controller.SetAxisValue(Xbox360Axis.RightThumbX, state.Gamepad.RightThumbX);
            virtualXbox360Controller.SetAxisValue(Xbox360Axis.RightThumbY, state.Gamepad.RightThumbY);

            virtualXbox360Controller.SubmitReport();
        }

        private void reportVirtualDualShock4Controller(IDualShock4Controller virtualDualShock4Controller, State state)
        {
            var wButtons = state.Gamepad.Buttons;

            bool up = (wButtons & GamepadButtonFlags.DPadUp) != 0;
            bool down = (wButtons & GamepadButtonFlags.DPadDown) != 0;
            bool left = (wButtons & GamepadButtonFlags.DPadLeft) != 0;
            bool right = (wButtons & GamepadButtonFlags.DPadRight) != 0;
            virtualDualShock4Controller.SetDPadDirection(MapToDualShock4DPad(up, down, left, right));

            virtualDualShock4Controller.SetButtonState(DualShock4Button.Options, (wButtons & GamepadButtonFlags.Start) != 0);
            virtualDualShock4Controller.SetButtonState(DualShock4SpecialButton.Touchpad, (wButtons & GamepadButtonFlags.Back) != 0);
            virtualDualShock4Controller.SetButtonState(DualShock4Button.ThumbLeft, (wButtons & GamepadButtonFlags.LeftThumb) != 0);
            virtualDualShock4Controller.SetButtonState(DualShock4Button.ThumbRight, (wButtons & GamepadButtonFlags.RightThumb) != 0);
            virtualDualShock4Controller.SetButtonState(DualShock4Button.ShoulderLeft, (wButtons & GamepadButtonFlags.LeftShoulder) != 0);
            virtualDualShock4Controller.SetButtonState(DualShock4Button.ShoulderRight, (wButtons & GamepadButtonFlags.RightShoulder) != 0);
            virtualDualShock4Controller.SetButtonState(DualShock4Button.Cross, (wButtons & GamepadButtonFlags.A) != 0);
            virtualDualShock4Controller.SetButtonState(DualShock4Button.Circle, (wButtons & GamepadButtonFlags.B) != 0);
            virtualDualShock4Controller.SetButtonState(DualShock4Button.Square, (wButtons & GamepadButtonFlags.X) != 0);
            virtualDualShock4Controller.SetButtonState(DualShock4Button.Triangle, (wButtons & GamepadButtonFlags.Y) != 0);

            virtualDualShock4Controller.SetButtonState(DualShock4Button.TriggerLeft, state.Gamepad.LeftTrigger > 0);
            virtualDualShock4Controller.SetButtonState(DualShock4Button.TriggerRight, state.Gamepad.RightTrigger > 0);

            virtualDualShock4Controller.SetSliderValue(DualShock4Slider.LeftTrigger, state.Gamepad.LeftTrigger);
            virtualDualShock4Controller.SetSliderValue(DualShock4Slider.RightTrigger, state.Gamepad.RightTrigger);

            virtualDualShock4Controller.SetAxisValue(DualShock4Axis.LeftThumbX, MapToByte(state.Gamepad.LeftThumbX));
            virtualDualShock4Controller.SetAxisValue(DualShock4Axis.LeftThumbY, MapToByte(state.Gamepad.LeftThumbY, true));
            virtualDualShock4Controller.SetAxisValue(DualShock4Axis.RightThumbX, MapToByte(state.Gamepad.RightThumbX));
            virtualDualShock4Controller.SetAxisValue(DualShock4Axis.RightThumbY, MapToByte(state.Gamepad.RightThumbY, true));

            virtualDualShock4Controller.SubmitReport();
        }

        private byte MapToByte(short num, bool reverse = false)
        {
            var percent = ((float)(num - short.MinValue)) / ushort.MaxValue;
            if (reverse)
            {
                percent = 1 - percent;
            }
            var result = (byte)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, Math.Round(percent * byte.MaxValue)));
            return result;
        }

        private byte MapToByte(ushort num, bool reverse = false)
        {
            var percent = ((float)num) / ushort.MaxValue;
            if (reverse)
            {
                percent = 1 - percent;
            }
            var result = (byte)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, Math.Round(percent * byte.MaxValue)));
            return result;
        }

        private ushort MapToUInt16(byte num, bool reverse = false)
        {
            var percent = ((float)num) / byte.MaxValue;
            if (reverse)
            {
                percent = 1 - percent;
            }
            var result = (ushort)Math.Min(ushort.MaxValue, Math.Max(ushort.MinValue, Math.Round(percent * ushort.MaxValue)));
            return result;
        }

        private DualShock4DPadDirection MapToDualShock4DPad(bool up, bool down, bool left, bool right)
        {
            if (!up && !down && !left && !right)
            {
                return DualShock4DPadDirection.None;
            }
            else if (up && !down && !left && !right)
            {
                return DualShock4DPadDirection.North;
            }
            else if (!up && down && !left && !right)
            {
                return DualShock4DPadDirection.South;
            }
            else if (!up && !down && left && !right)
            {
                return DualShock4DPadDirection.West;
            }
            else if (!up && !down && !left && right)
            {
                return DualShock4DPadDirection.East;
            }
            else if (up && !down && !left && right)
            {
                return DualShock4DPadDirection.Northeast;
            }
            else if (!up && down && !left && right)
            {
                return DualShock4DPadDirection.Southeast;
            }
            else if (!up && down && left && !right)
            {
                return DualShock4DPadDirection.Southwest;
            }
            else if (up && !down && left && !right)
            {
                return DualShock4DPadDirection.Northwest;
            }
            else
            {
                return DualShock4DPadDirection.None;
            }
        }

        private void UpdateSwitchVirtualController(GamepadType gamepadType, int index, bool enabled)
        {
            if (gamepadType == GamepadType.DualShock4)
            {
                if (virtualDualShock4Controllers[index] == null && enabled == true)
                {
                    var controller = viGEmClient.CreateDualShock4Controller();
                    controller.AutoSubmitReport = false;
                    controller.FeedbackReceived += (object sender, DualShock4FeedbackReceivedEventArgs e) =>
                    {
                        var selectRealControler = realControllers[index];
                        if (selectRealControler.IsConnected)
                        {
                            var vibration = new Vibration
                            {
                                LeftMotorSpeed = MapToUInt16(e.LargeMotor),
                                RightMotorSpeed = MapToUInt16(e.SmallMotor)
                            };
                            selectRealControler.SetVibration(vibration);
                        }
                    };
                    controller.Connect();
                    virtualDualShock4Controllers[index] = controller;
                }
                else if (virtualDualShock4Controllers[index] != null && enabled == false)
                {
                    virtualDualShock4Controllers[index].Disconnect();
                    virtualDualShock4Controllers[index] = null;
                }
            }
            else
            {
                if (virtualXbox360Controllers[index] == null && enabled == true)
                {
                    var controller = viGEmClient.CreateXbox360Controller();
                    controller.AutoSubmitReport = false;
                    controller.FeedbackReceived += (object sender, Xbox360FeedbackReceivedEventArgs e) =>
                    {
                        var selectRealControler = realControllers[index];
                        if (selectRealControler.IsConnected)
                        {
                            var vibration = new Vibration
                            {
                                LeftMotorSpeed = MapToUInt16(e.LargeMotor),
                                RightMotorSpeed = MapToUInt16(e.SmallMotor)
                            };
                            selectRealControler.SetVibration(vibration);
                        }
                    };
                    controller.Connect();
                    virtualXbox360Controllers[index] = controller;
                }
                else if (virtualXbox360Controllers[index] != null && enabled == false)
                {
                    virtualXbox360Controllers[index].Disconnect();
                    virtualXbox360Controllers[index] = null;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopGamepadChecker();
        }

        private void UpdateGamepadStatusUI(string[] messages)
        {
            var action = new Action(() => {
                StringBuilder stringBuilder = new StringBuilder();
                int count = 0;
                foreach (var message in messages)
                {
                    count++;
                    stringBuilder.AppendLine($"控制器{count}: {message}");
                }
                statusLabel.Text = stringBuilder.ToString();
            });
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void StartGamepadChecker()
        {
            cancellationSource = new CancellationTokenSource();
            var gamepadType = dualShock4RadioButton.Checked ? GamepadType.DualShock4 : GamepadType.Xbox360;
            byte controllersBitMask = 0b1111; // 全部四个手柄
            if (gamepadType == GamepadType.Xbox360)
            {
                // 防止环回，只使用目前接入的
                controllersBitMask = 0b0000;
                foreach (var selectRealControler in realControllers)
                {
                    int index = (int)selectRealControler.UserIndex;
                    if (selectRealControler.IsConnected)
                    {
                        controllersBitMask += (byte)(0b1 << index);
                    }
                }
            }
            _ = GamepadChecker(gamepadType, controllersBitMask, cancellationSource.Token); 
            typeSelectGroupBox.Enabled = false;
            stopButton.Enabled = true;
            startButton.Enabled = false;
        }

        private void StopGamepadChecker()
        {
            if (cancellationSource != null)
            {
                cancellationSource.Cancel();
            }
            typeSelectGroupBox.Enabled = true;
            stopButton.Enabled = false;
            startButton.Enabled = true;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartGamepadChecker();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopGamepadChecker();
        }

        private void statusCheckTimer_Tick(object sender, EventArgs e)
        {
            var messagesList = new List<string>();
            foreach (var selectRealControler in realControllers)
            {
                int index = (int)selectRealControler.UserIndex;
                if (selectRealControler.IsConnected)
                {
                    messagesList.Add("已连接");
                }
                else
                {
                    messagesList.Add("未连接");
                }
            }
            UpdateGamepadStatusUI(messagesList.ToArray());
        }
    }
}
