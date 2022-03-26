// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"


int hello_world()
{
	return VML_HA;
}

// Timer methods implementation
void Timer::reset()
{
	time = clock_t::now();
}

double Timer::elapsed() const
{
	return std::chrono::duration_cast<second_t>(clock_t::now() - time).count();
}


int
calculate_function(int length, double vector[], int function_code,
	double res_HA[], double res_LA[], double res_EP[], double time[])
{
	long long mode;
	if (function_code == vmdSin_code)
	{
		mode = VML_HA;
		Timer timer;
		vmdSin(length, vector, res_HA, mode);
		time[0] = timer.elapsed();

		mode = VML_LA;
		timer.reset();
		vmdSin(length, vector, res_LA, mode);
		time[1] = timer.elapsed();

		mode = VML_EP;
		timer.reset();
		vmdSin(length, vector, res_EP, mode);
		time[2] = timer.elapsed();

		return 0;
	}
	else if (function_code == vmdCos_code) 
	{
		mode = VML_HA;
		Timer timer;
		vmdCos(length, vector, res_HA, mode);
		time[0] = timer.elapsed();

		mode = VML_LA;
		timer.reset();
		vmdCos(length, vector, res_LA, mode);
		time[1] = timer.elapsed();

		mode = VML_EP;
		timer.reset();
		vmdCos(length, vector, res_EP, mode);
		time[2] = timer.elapsed();

		return 0;
	}
	else if (function_code == vmdSinCos_code) 
	{
		mode = VML_HA;
		Timer timer;
		vmdSinCos(length, vector, res_HA, NULL, mode);
		time[0] = timer.elapsed();

		mode = VML_LA;
		timer.reset();
		vmdSinCos(length, vector, res_LA, NULL, mode);
		time[1] = timer.elapsed();

		mode = VML_EP;
		timer.reset();
		vmdSinCos(length, vector, res_EP, NULL, mode);
		time[2] = timer.elapsed();

		return 0;
	}
	else 
	{
		return WRONG_FUNCTION;
	}
}