# sle44xx_console_demo
SLE44xx/FM44xx synchronous smartcard demo, this project is based on [HID Global OK Sample code](https://github.com/hidglobal/HID-OMNIKEY-Sample-Codes), with following ehancement:

- We can choose Non OK Readers that supports 'Vendor-specific synchronous command set'
- SLE44xx PSC can input from console before Update Main Memory
- Added dedicated SLE44xx Verification
- Read Memory Data can be dump as RAM Buffer
- AT24Cxx Cards can be choosen from a menu list
- Write/Update Memory Data can be Randomized

see following pictures:

1. Reader Selection
```cmd
Smart Card Readers

  0. Back
  1. Refresh devices list
  2. PCSC Reader Name: HID Global OMNIKEY 3x21 Smart Card Reader 0
  3. PCSC Reader Name: HID Global OMNIKEY 5422 Smartcard Reader 0
  4. PCSC Reader Name: HID Global OMNIKEY 5422CL Smartcard Reader 0
```

2. Synchronus Card Selection
```cmd
Synchronus Contact Card Examples

  0. Back
  1. 2WBP Example(SLE 4452/42/FM4442)
  2. 3WBP Example(SLE 4418/28/FM4428)
  3. I2C Example(AT24C01/02...)
```

3. Dedicated Verify
```cmd
2WBP Example(SLE 4452/42/FM4442)

  0. Back
  1. Read Main Memory
  2. Read Protection Memory
  3. Verify
  4. Update Main Memory

>>>

3WBP Example(SLE 4418/28/FM4428)

  0. Back
  1. Read Main Memory
  2. Verify
  3. Update Main Memory

>>>
```

4. I2C Card List
```cmd
I2C Example(AT24C01/02...)

I2C Example(AT24C01/02...)

  0. Back
  1. Read Example for AT24C01A
  2. Write Example for AT24C01A
  3. Read Example for AT24C02
  4. Write Example for AT24C02
  5. Read Example for AT24C04
  6. Write Example for AT24C04
  7. Read Example for AT24C08
  8. Write Example for AT24C08
  9. Read Example for AT24C16
 10. Write Example for AT24C16
 11. Read Example for AT24C164
 12. Write Example for AT24C164
 13. Read Example for AT24C32
 14. Write Example for AT24C32
 15. Read Example for AT24C64
 16. Write Example for AT24C64
 17. Read Example for AT24C128
 18. Write Example for AT24C128
 19. Read Example for AT24C256
 20. Write Example for AT24C256
 21. Read Example for AT24CS128
 22. Write Example for AT24CS128
 23. Read Example for AT24CS256
 24. Write Example for AT24CS256
 25. Read Example for AT24C512
 26. Write Example for AT24C512
 27. Read Example for AT24C1024
 28. Write Example for AT24C1024

>>>

>>> 3
Read Example for AT24C02, 256 Bytes.
0000: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
0010: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
0020: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
0030: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
0040: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
0050: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
0060: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
0070: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
0080: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
0090: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
00A0: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
00B0: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
00C0: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
00D0: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
00E0: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
00F0: AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE AE ................
Press any key to continue..

>>> 4
Write Example for AT24C02, 256 Bytes.
Data Pattern 2020202020202020
Press any key to continue..
```

---
by .NFC 2024/11/02
