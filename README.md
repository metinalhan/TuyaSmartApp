- # TuyaSmartApp

You need to grant the permissions mentioned below for your projects in your Tuya developer account.
- IoT Core
- Authorization Token Management
- Smart Home Basic Service


[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)  
**TuyaSmartApp** is a smart home application built on the Tuya platform. It allows users to control and manage Tuya-supported devices effortlessly.

---

## 🚀 Features
- Control and monitor Tuya-supported devices.
- User-friendly and modern interface.
- Device management (add, delete, and edit).
- Secure communication via API.
- Local and cloud-based control options.

---

## 📋 Requirements
To run this project, you will need the following tools and environment:
- **.NET SDK** (6.0 or later)
- **Visual Studio 2022** or a compatible IDE
- **Tuya Developer Account** (for API access)
- Internet connection

---

## 🛠️ Installation
Follow these steps to set up and run the project:

1. **Clone the Repository**
```
git clone https://github.com/metinalhan/TuyaSmartApp.git
cd TuyaSmartApp
```

3. **Install Dependencies**
Restore the required NuGet packages:
```
dotnet restore
```

4. **Set Up API Keys**
Open the appsettings.json file and add your Tuya API keys in the following format:
```json
{
    "TuyaApi": {
        "ClientId": "YOUR_CLIENT_ID",
        "ClientSecret": "YOUR_CLIENT_SECRET"
    }
}
```

6. **Run the Project**
```
dotnet run
```

Usage
Log in with your Tuya account.
Add new devices or manage existing ones.
Control your devices and set up automation rules.

Contributing
Contributions are welcome! 🎉
To contribute, follow these steps:

Fork the repository.
Create a new branch (git checkout -b feature-branch).
Make your changes and commit them (git commit -m 'Add new feature').
Push your changes to the branch (git push origin feature-branch).
Open a pull request.

License
This project is licensed under the MIT License.

Contact
For any questions or suggestions, feel free to reach out:
Metin Alhan

GitHub: metinalhan
Email: metinalhan@gmail.com


