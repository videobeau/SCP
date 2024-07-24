#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
	char *filepath = argv[1];

	if (argc <= 1)
	{
		printf("usage:\nscp [filepath]\n");
		return -1;
	}

	FILE *inputFile;
	unsigned char *fileDataBuffer;
	unsigned long fileSizeInBytes;

	inputFile = fopen(filepath, "rb");

	// Check that file exists
	if (inputFile == NULL)
	{
		perror(strcat(filepath, " does not exist\n"));
		return -1;
	}

	// Get File Size
	fseek(inputFile, 0, SEEK_END);
	fileSizeInBytes = ftell(inputFile);
	fseek(inputFile, 0, SEEK_SET);

	fileDataBuffer = (char *)malloc(fileSizeInBytes * sizeof(char));
	if (fileDataBuffer == NULL)
	{
		perror("Memory allocation failed\n");
		return -1;
	}

	// Read File
	fread(fileDataBuffer, sizeof(char), fileSizeInBytes, inputFile);

	// Print each byte (in hexadecimal format)
	for (long i = 0; i < fileSizeInBytes; i++)
	{
		printf("%0i ", fileDataBuffer[i]);
	}
	printf("\n");

	// Clean up
	fclose(inputFile);
	free(fileDataBuffer);

	return 0;
}
