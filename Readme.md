# DotnetRuntimeBootstrapper

[![Build](https://github.com/Tyrrrz/DotnetRuntimeBootstrapper/workflows/CI/badge.svg?branch=master)](https://github.com/Tyrrrz/DotnetRuntimeBootstrapper/actions)
[![Coverage](https://codecov.io/gh/Tyrrrz/DotnetRuntimeBootstrapper/branch/master/graph/badge.svg)](https://codecov.io/gh/Tyrrrz/DotnetRuntimeBootstrapper)
[![Version](https://img.shields.io/nuget/v/DotnetRuntimeBootstrapper.svg)](https://nuget.org/packages/DotnetRuntimeBootstrapper)
[![Downloads](https://img.shields.io/nuget/dt/DotnetRuntimeBootstrapper.svg)](https://nuget.org/packages/DotnetRuntimeBootstrapper)
[![Donate](https://img.shields.io/badge/donate-$$$-purple.svg)](https://tyrrrz.me/donate)

✅ **Project status: active**.

DotnetRuntimeBootstrapper replaces the default `exe` shim generated by MSBuild for Windows executables with a fully featured bootstrapper that can download and install .NET runtime and other missing components required by your application.

> Designed primarily for bootstrapping desktop applications. Might not work optimally for other use cases.

## Download

📦 [NuGet](https://nuget.org/packages/DotnetRuntimeBootstrapper): `dotnet add package DotnetRuntimeBootstrapper`

## Features

- Acts as a tight shim around target assembly
- Installs .NET runtime version required by the application
- Installs Visual C++ redistributable binaries, if missing
- Installs required Windows updates, if missing
- Includes basic GUI to guide the user
- Works out-of-the-box on Windows 7 and higher

## Video

https://user-images.githubusercontent.com/1935960/120946093-1dd6d000-c6f0-11eb-9cfe-a373a00b4888.mp4

## Usage

### Build integration

To add DotnetRuntimeBootstrapper to your project, simply install the corresponding [NuGet package](https://nuget.org/packages/DotnetRuntimeBootstrapper).
MSBuild will automatically pick up the `props` and `targets` files provided by the package and integrate them inside the build process.

Once that's done, building or publishing the project should produce two additional files in the output directory:

```ini
MyApp.exe                 <-- bootstrapper executable
MyApp.exe.config          <-- runtime manifest required by the executable
MyApp.dll
MyApp.pdb
MyApp.deps.json
MyApp.runtimeconfig.json
```

Running `MyApp.exe` ensures that the required version of .NET runtime is available and then delegates execution to the target assembly through the .NET CLI.
If the runtime or any of its prerequisites are not found, then the user is prompted with an option to download and install the missing components automatically.

> ⚠️ Make sure to include the `.config` file when distributing your application.
Bootstrapper executable may not be able to run properly without it.

### How it works

Bootstrapper executable is a pre-compiled binary that runs on legacy .NET Framework.
It targets .NET Framework v3.5 but can also roll over to later versions of the framework if the corresponding manifest file is present.
This ensures that the bootstrapper can run out-of-the-box on all systems starting with Windows 7.

During build, the bootstrapper and the manifest file are copied to the output directory by a custom MSBuild task provided through the package.
A special configuration file, instructing which assembly to run and which version of the runtime is required, is injected as an embedded resource inside the bootstrapper as part of the build process.

Any file metadata present on the target assembly, such as `ProductName`, `FileDescription`, `FileVersion`, `ProductVersion`, `LegalCopyright`, etc. gets copied to the bootstrapper executable.
If there is an icon specified on the project through the `<ApplicationIcon>` property, it is added as well.