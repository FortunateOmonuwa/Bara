"use client";

import Image from "next/image";

export default function RegisterPage() {
  return (
    <main className="min-h-screen flex items-center justify-center bg-[#1a0000] px-4">
      {/* White container card */}
      <div className="bg-white rounded-lg shadow-lg flex flex-col md:flex-row w-full max-w-4xl  h-[550px]">
        {/* Left: Form Section */}
        <div className="flex-1  md:p-12">
          {/* Logo */}
          <div className="mb-8">
            <Image
              src="/logo.png"
              alt="Bara Logo"
              width={60}
              height={40}
              className="h-auto w-auto"
            />
          </div>

          {/* Heading */}
          <h1 className="text-xl font-semibold mb-6">Create a Bara account</h1>

          {/* Google Button */}
          <button className="w-full bg-[#800000] text-white font-medium py-3 rounded-md hover:bg-[#BF0000] flex items-center justify-center gap-2">
            <Image
              src="/Google.png"
              alt="Google Icon"
              width={20}
              height={20}
            />
            Create with Google
          </button>

          {/* Divider */}
          <div className="flex items-center my-6">
            <div className="flex-grow h-px bg-gray-300"></div>
            <span className="px-3 text-sm text-gray-500">or</span>
            <div className="flex-grow h-px bg-gray-300"></div>
          </div>

          {/* Email Input */}
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Email
          </label>
          <input
            type="email"
            placeholder="Enter your email"
            className="w-full border border-gray-300 rounded-md px-3 py-3 focus:outline-none focus:ring-1 focus:ring-[#800000] focus:border-[#800000] mb-4"
          />

          {/* Continue Button */}
          <button className="w-full bg-[#800000] text-white font-medium py-3 rounded-md hover:bg-[#BF0000] flex items-center justify-center gap-2">
            Continue
            <span className="ml-2">→</span>
          </button>

          {/* Terms */}
          <div className="flex items-start mt-4">
            <input type="checkbox" id="terms" className="mt-1 mr-2" />
            <label htmlFor="terms" className="text-xs text-gray-500">
              By checking this box, you agree to the IP policy and Terms of use
              of Bara.
            </label>
          </div>
        </div>

        {/* Right: Image Section */}
        <div className="md:w-1/2 relative">
          <Image
            src="/Mask group.png"
            alt="Register Illustration"
            width={500}
            height={500}
            className="w-full h-full object-cover"
          />
        </div>
      </div>
    </main>
  );
}
