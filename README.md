
# JITK - JIT Killer
[![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](https://github.com/tterb/atomic-design-ui/blob/master/LICENSES)
![files](https://img.shields.io/github/directory-file-count/polynomen/JITK)

```
       _ _____ _______ _  __
      | |_   _|__   __| |/ /
      | | | |    | |  | ' / 
  _   | | | |    | |  |  <  
 | |__| |_| |_   | |  | . \ 
  \____/|_____|  |_|  |_|\_\
                            
        by polynomen
```

JIT Killer is hooker for clrjit.dll

This process presents the communication between the compiler and the program to the user.

## Features

- Breakpoint
- Disassemble from Hex Codes
- User friendly design
  
## Demo

![Rec 0003](https://user-images.githubusercontent.com/54905232/177761753-f11b5ea9-c5b1-4353-98d5-ec272b13bd58.gif)

## Commands

| Parameter | Description                |
| -------- | ------------------------- |
| `quit` | Suspend JIT Killer |
| `infoa` | Get assembly info of target module. |
| `clear` | Clear Console |
| `help` | Help |
| `about` |  About **JITK** |
| `infof` |  Get functions from loaded module. |
| `fc` |  Path Clear |
| `f` |   Assign File |
| `fg` |  Assign File Argument/s |
| `c` |  Continue or Start Hook Process. |
| `g` |  Step next function. |
| `b` |  Define breakpoint |
| `bl` |  List All Breakpoint |
| `disas` |   Disassemble Method |
| `disash` |   Print method body as hex |


  
## Thanks

- [@Washi1337](https://www.github.com/Washi1337) For the information it gives about the compiler and the problems it solves.
- [@shibayan](https://www.github.com/shibayan) For [Sharprompt](https://github.com/shibayan/Sharprompt) and for quickly merging my improvements on Sharprompt
- [@0xd4d](https://www.github.com/0xd4d) For [dnlib](https://github.com/0xd4d/dnlib)
- [@maddnias](https://github.com/maddnias) For [SJITHook](https://github.com/maddnias/SJITHook)
- All RTN members

  
