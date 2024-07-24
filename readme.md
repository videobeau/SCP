# Building
Open **Developer Command Prompt for VS 2022**

cd to this directory
```cmd
cd C:\Users\...\SCP\
```
build the project with `cl`
```cmd
cl main.c -o scp
```
then run `scp` with the filepath of the png you are converting to scp
```cmd
scp [pngfilepath.png]
```
# PNG spec
first 8 bytes are always: `137 80 78 71 13 10 26 10`

`IHDR Chunk`

`... other chunks ...`

`IEND chunk`

## Chunk
`4 byte uint` number of bytes in the data field

`4 byte chunk type`

`Chunk Data`

`4 byte CRC` cyclic redundancy check

# 🗒️ General C Notes
## Printing Arguments
This will print all the arguments entered into the program. The program itself will be at `argv[0]`
```c
int main(int argc, char *argv[])
{
	for (int i = 0; i < argc; i++)
	{
		printf("%i: %s\n", i, argv[i]);
	}
}
```

## fseek vs rewind
folks online are saying `fseek()` isn't portable and to use `rewind()` instead
