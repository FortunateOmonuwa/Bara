"use client";

import { useState } from "react";
import { useRouter } from "next/navigation"; 
import Image from "next/image";

export default function VerifyEmailPage() {
  const [otp, setOtp] = useState("");
  const router = useRouter();

  // only allow digits and max length 6
  const handleOtpChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value.replace(/\D/g, ""); // remove non-digits
    if (value.length <= 6) {
      setOtp(value);
    }
  };

  // Continue button is enabled only when OTP length is 6
  const canContinue = otp.length === 6;

  const handleContinue = () => {
    if (canContinue) {
      router.push("/auth/password"); // navigates to set password page
    }
  };

  return (
    <main className="min-h-screen flex items-center justify-center bg-[#1a0000] px-4">
      {/* White container card */}
      <div className="bg-white rounded-lg shadow-lg flex flex-col md:flex-row w-full max-w-4xl h-[550px] relative">
        {/* Left: Form Section */}
        <div className="flex-1 md:p-12 overflow-y-auto">
          {/* Logo */}
          <div>
            <Image
              src="/logo.png"
              alt="Bara Logo"
              width={60}
              height={40}
              className="h-auto w-auto"
            />
          </div>

          {/* Heading */}
          <h1 className="text-2xl font-semibold mb-2 text-[#22242A]">
            Verify your email
          </h1>

          {/* Description */}
          <p className="text-sm text-[#333740] mb-2 leading-relaxed">
            An OTP has been sent to{" "}
            <span className="font-semibold">janedoe@gmail.com</span>, input the
            code in the field provided below.
          </p>

          {/* OTP Input */}
          <label className="block text-sm font-medium text-[#22242A] mb-2">
            OTP
          </label>
          <input
            type="text"
            placeholder="Enter OTP"
            value={otp}
            onChange={handleOtpChange}
            className="w-full border border-[#ABADB2] rounded-md px-3 py-3 mb-3 bg-white focus:outline-none focus:ring-1 focus:ring-[#800000] focus:border-[#800000]"
          />

          {/* Continue Button */}
          <button
            onClick={handleContinue}
            disabled={!canContinue}
            className={`w-full font-medium py-3 rounded-md flex items-center justify-center gap-2 transition-colors ${
              canContinue
                ? "bg-[#800000] text-white hover:bg-[#BF0000]"
                : "bg-[#F5F5F5] text-[#858990] cursor-not-allowed"
            }`}
          >
            Continue
            <span className="ml-2 text-lg">→</span>
          </button>

          {/* Success message when 6 digits are entered */}
          {otp.length === 6 && (
            <div className="mx-auto mt-28 w-64 flex items-center justify-center border border-[#0DA500] rounded-md px-2 py-2 text-[#0DA500] text-sm font-medium gap-2">
              <Image
                src="/Check_ring.png"
                alt="Success Icon"
                width={16}
                height={16}
                className="object-contain"
              />
              Email successfully verified!
            </div>
          )}
        </div>

        {/* Right: Image Section */}
        <div className="md:w-1/2 relative hidden md:flex items-center justify-center p-8">
          <Image
            src="/Mask group.png"
            alt="Verify Illustration"
            width={350}
            height={350}
            className="object-contain"
          />
        </div>
      </div>
    </main>
  );
}
