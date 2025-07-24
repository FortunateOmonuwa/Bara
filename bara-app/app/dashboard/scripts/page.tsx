"use client";

import Image from "next/image";
import { useState } from "react";
import DashboardNavbar from "@/components/DashboardNavbar";
import WriterProfileCard from "@/components/WriterProfileCard";

export default function ScriptPage() {

  const [copied, setCopied] = useState(false);
  const scriptLink = "https://your-script-link.com"; 

  const handleCopy = async () => {
    await navigator.clipboard.writeText(scriptLink);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <main className="min-h-screen bg-[#F9FAFB]">
      <DashboardNavbar />

      <div className="max-w-7xl mx-auto px-4 py-8 grid grid-cols-1 lg:grid-cols-12 gap-8">
        {/* LEFT SIDE */}
        <div className="lg:col-span-3 space-y-4">
          <h1 className="text-xl font-semibold text-[#22242A]">
            Broken Promise
          </h1>
          <Image
            src="/flowery.png"
            alt="Broken Promise"
            width={400}
            height={300}
            className="w-full rounded-md object-cover"
          />
          <p className="text-lg font-semibold text-[#22242A]">₦300,000.00</p>

          <button className="w-full border border-[#F5C6C6] bg-[#FFF1F1] text-[#810306] text-sm font-medium py-2 rounded-md hover:bg-[#ffe1e1] transition">
            <span className="inline-flex items-center gap-2 justify-center">
              <Image src="/heart.png" alt="Save" width={16} height={16} />
              Save this script
            </span>
          </button>
          <hr className="border-t border-gray-300 my-2" />
          {/* Copy script section */}
          <div>
            <p className="text-sm text-[#333740] mb-1">Copy this script</p>

            {/* Wrapper for icon and feedback */}
            <div className="relative inline-flex items-center">
              <button
                onClick={handleCopy}
                className="p-1 hover:opacity-80"
                aria-label="Copy link"
              >
                <Image src="/copy.png" alt="Copy" width={20} height={20} />
              </button>

              {/* Copied feedback directly below icon */}
              {copied && (
                <span className="absolute left-1/2 -translate-x-1/2 top-full mt-1 text-xs text-green-600 whitespace-nowrap">
                  Copied!
                </span>
              )}
            </div>
          </div>
        </div>

        {/* MIDDLE SECTION */}
        <div className="lg:col-span-6 space-y-4">
          <div className="bg-white border border-gray-200 rounded-md p-4 space-y-4">
            <h2 className="font-semibold text-lg text-[#22242A]">Logline</h2>
            <p className="text-sm text-[#333740]">
              A desperate journalist uncovers a hidden AI network controlling
              world events and must race against time to expose the truth before
              becoming its next target.
            </p>

            <div className="flex gap-2">
              <button className="flex-1 border border-gray-300 py-2 rounded-md text-sm font-medium bg-[#F9F9F9] text-[#22242A] flex items-center justify-center gap-2">
                <Image
                  src="/synopsis.png"
                  alt="Synopsis"
                  width={16}
                  height={16}
                />
                Synopsis
              </button>
              <button className="flex-1 border border-gray-300 py-2 rounded-md text-sm font-medium text-[#22242A] hover:bg-gray-50 flex items-center justify-center gap-2">
                <Image src="/script.png" alt="Script" width={16} height={16} />
                Script
              </button>
            </div>

            {/* Payment methods */}
            <div className="pt-4 space-y-3">
              <h3 className="font-semibold text-sm text-[#22242A]">
                Payment methods
              </h3>

              <div className="flex items-center gap-2 text-sm text-[#333740] opacity-50">
                <input type="radio" name="pay" disabled />
                NO Wallet
              </div>

              <div className="flex items-center justify-between text-sm text-[#333740] border border-gray-300 rounded-md py-2 px-3">
                <div className="flex items-center gap-2">
                  <input type="radio" name="pay" defaultChecked />
                  ****1243
                </div>
                <button>
                  <Image
                    src="/delete.png"
                    alt="Remove"
                    width={16}
                    height={16}
                  />
                </button>
              </div>

              <button className="text-sm font-medium text-[#810306] hover:underline">
                + Add new card
              </button>

              <button className="w-full bg-[#800000] text-white py-2 rounded-md text-sm font-medium hover:bg-[#4d0000] transition">
                Make payment
              </button>

              <div className="flex items-start gap-2 mt-2">
                <input type="checkbox" className="mt-1 accent-[#800000]" />
                <p className="text-xs text-[#333740]">
                  By checking this box, you agree to the Non‑Disclosure
                  Agreement, committing not to share, misuse, or reproduce the
                  information in this synopsis. Your payment will be securely
                  held by Bara. You have 14 days from today to review the
                  script, engage with the writer, and confirm the script.
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* RIGHT SIDE */}
        <div className="lg:col-span-3">
          <WriterProfileCard />
        </div>
      </div>
    </main>
  );
}
