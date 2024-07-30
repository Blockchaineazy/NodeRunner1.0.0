# NodeRunner

## Disclaimer

This project is not affiliated with, endorsed by, or connected to Hytopia/Hychain in any way.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

## Description

NodeRunner is a Windows application built with FluentAvalonia on Visual Studio 2022. It allows users to run their Hychain nodes, check balances, and claim rewards.

## Installation

1. Follow the official Guardian Node Software setup here.
2. Rename your Guardian Node folder to "scripts".
3. Make sure your script files are named as follows:
    - start.bat (script that initiates the nodes)
    - check.bat (script that checks the balances)
    - claim.bat (script that claims your pending balance)
4. Clone the repository: `git clone https://github.com/Blockchaineazy/NodeRunner1.0.0.git`
5. Place the "scripts" folder in the NodeRunner project folder (optional as you will select it in the app later).
6. Open Visual Studio 2022.
7. In Visual Studio, go to `File > Open > Project/Solution`.
8. Navigate to the directory where you cloned the NodeRunner repository and select the `.sln` file to open the project.
9. Install the following packages via NuGet Package Manager:
    - Avalonia 11.1.1
    - Avalonia.Desktop 11.1.1
    - Avalonia.Diagnostics 11.1.1
    - Avalonia.Fonts.Inter 11.1.1
    - FluentAvalonia 2.0.5

## Build (Required)

1. Make sure you have .NET installed on your machine. If not, you can download it from the official .NET website.
2. In Visual Studio, go to `Build > Build Solution`. This will create an executable file in the `bin/Debug` or `bin/Release` folder within your project directory.

## Publish (Optional)

1. In Visual Studio, go to `Build > Publish NodeRunner`. This will create a self-contained executable file in the `publish` folder within your project directory. This file can be run on any machine, even if it doesn't have .NET installed.

## Usage

1. In the NodeRunner application, click on "Select Scripts Folder" and choose the "scripts" folder that you placed in the NodeRunner project folder.
2. Run NodeRunner.exe.
3. Use the "Run Nodes" button to start running nodes.
4. Use the "Check Balance" button to check the balance of your nodes.
5. Use the "Claim" button to claim rewards.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue.

## License

This project is licensed under the Creative Commons Attribution-NonCommercial 4.0 International License. To view a copy of this license, visit http://creativecommons.org/licenses/by-nc/4.0/.

## Contact
https://www.blockchaineazy.box
