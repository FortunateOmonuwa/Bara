"use client";

import Image from "next/image";
import { useState } from "react";

export default function WriterProfileCard() {
  const [copied, setCopied] = useState(false);
  const portfolioLink = "https://timothy-edwards.com/works";

  const handleCopy = async () => {
    await navigator.clipboard.writeText(portfolioLink);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <div className="bg-white border border-gray-300 rounded-lg p-4 w-[300px] mx-auto space-y-3">
      {/* Title */}
      <h3 className="font-semibold text-lg text-[#22242A] text-center">
        Writer&apos;s profile
      </h3>

      {/* Profile image (center) */}
      <div className="flex justify-center">
        <Image
          src="/writer.png"
          alt="Writer"
          width={80}
          height={80}
          className="rounded-full object-cover"
        />
      </div>

      {/* Name and description */}
      <div className="p-1 space-y-2">
        <h4 className="font-bold text-[20px] text-[#22242A]">
          Timothy Edwards
        </h4>

        <p className="text-sm text-[#333740] max-w-[250px] leading-snug">
          Award-winning writer and motivational speaker. Worked with a lot of
          top brands.
        </p>
      </div>

      {/* Portfolio section */}
      <div className="p-1 space-y-2">
        <h2 className="font-semibold text-[15px] text-[#22242A]">Portfolio</h2>

        <div className="flex items-center justify-start space-x-2">
          {/* Link with its own border */}
          <a
            href={portfolioLink}
            target="_blank"
            rel="noopener noreferrer"
            className="text-sm text-[#000AAF] underline border border-[#ABADB2] px-4 py-3 rounded-md whitespace-nowrap"
          >
            Timothy-edwards.com/works
          </a>

          {/* Copy button and feedback */}
          <div className="relative flex-shrink-0">
            <button
              onClick={handleCopy}
              className="border border-[#ABADB2] p-3 rounded-md flex items-center justify-center"
            >
              <Image src="/Copy.png" alt="Copy" width={16} height={16} />
            </button>

            {copied && (
              <span className="absolute left-1/2 -translate-x-1/2 mt-1 text-xs text-green-600 whitespace-nowrap">
                Copied!
              </span>
            )}
          </div>
        </div>
      </div>

      {/* View profile button */}
      <button className="w-full border border-[#810306] text-[#810306] text-sm font-medium mt-2 py-2 rounded-md hover:bg-[#fff5f5] transition">
        View profile
      </button>
    </div>
  );
}
