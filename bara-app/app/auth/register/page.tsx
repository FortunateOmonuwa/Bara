"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Image from "next/image";

export default function RegisterPage() {
  const [email, setEmail] = useState("");
  const [termsChecked, setTermsChecked] = useState(false);
  const [googleClicked, setGoogleClicked] = useState(false);

  const router = useRouter();

  // Determine if continue button should be enabled
  const canContinue = (googleClicked || email.trim() !== "") && termsChecked;

  const handleGoogleClick = () => {
    setGoogleClicked(true);
  };
 

  const handleContinue = () => {

    if (canContinue) {
      router.push("/auth/verify-email"); 
    }
  };

  return (
    <main className="min-h-screen flex items-center justify-center bg-[#1a0000] px-4">
      {/* White container card */}
      <div className="bg-white rounded-lg shadow-lg flex flex-col md:flex-row w-full max-w-4xl h-[550px]">
        {/* Left: Form Section */}
        <div className="flex-1 md:p-12 overflow-y-auto">
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
          <h1 className="text-xl font-semibold mb-6 text-[#22242A]">
            Create a Bara account
          </h1>

          {/* Google Button */}
          <button
            onClick={handleGoogleClick}
            className="w-full bg-[#800000] text-white font-medium py-3 rounded-md hover:bg-[#1a0000] flex items-center justify-center gap-4"
          >
            <Image src="/Google.png" alt="Google Icon" width={20} height={20} />
            Create with Google
          </button>

          {/* Divider */}
          <div className="flex items-center justify-center my-4">
            <span className="text-sm text-[#333740]">or</span>
          </div>

          {/* Email Input */}
          <label className="block text-sm font-medium text-[#22242A] mb-2">
            Email
          </label>
          <input
            type="email"
            placeholder="Enter your email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full border border-[#ABADB2] rounded-md px-3 py-3 mb-4 bg-white focus:outline-none focus:ring-1 focus:ring-[#800000] focus:border-[#800000]"
          />

          {/* Continue Button */}
          <button
            onClick={handleContinue}
            disabled={!canContinue}
            className={`w-full font-medium py-3 rounded-md flex items-center justify-center gap-2 transition-colors ${
              canContinue
                ? "bg-[#800000] text-white hover:bg-[#1a0000]"
                : "bg-[#F5F5F5] text-[#858990] cursor-not-allowed"
            }`}
          >
            Continue
            <span className="ml-2 text-lg">→</span>
          </button>

          {/* Terms */}
          <div className="flex items-start mt-4">
            <input
              type="checkbox"
              id="terms"
              className="mt-1 mr-2 accent-[#810306]"
              checked={termsChecked}
              onChange={(e) => setTermsChecked(e.target.checked)}
            />
            <label htmlFor="terms" className="text-xs text-[#333740]">
              By checking this box, you agree to the IP policy and Terms of use
              of Bara.
            </label>
          </div>
        </div>

        {/* Right: Image Section */}
        <div className="md:w-1/2 relative hidden md:flex items-center justify-center p-8">
          <Image
            src="/Mask group.png"
            alt="Register Illustration"
            width={350}
            height={350}
            className="object-contain"
          />
        </div>
      </div>
    </main>
  );
}
