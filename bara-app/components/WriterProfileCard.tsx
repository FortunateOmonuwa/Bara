"use client";

import Image from "next/image";

export default function WriterProfileCard() {
  return (
    <div className="bg-white border border-gray-200 rounded-md p-4 space-y-4">
      <h3 className="font-semibold text-sm text-[#22242A]">
        Writer&apos;s profile
      </h3>

      <div className="flex justify-center">
        <Image
          src="/writer.png"
          alt="Writer"
          width={80}
          height={80}
          className="rounded-full object-cover"
        />
      </div>

      <p className="font-medium text-sm text-[#22242A]">Timothy Edwards</p>
      <p className="text-xs text-[#333740]">
        Award-winning writer and motivational speaker. Worked with a lot of top
        brands.
      </p>

      <a
        href="https://timothy-edwards.com/works"
        target="_blank"
        rel="noopener noreferrer"
        className="block text-sm text-[#810306] underline"
      >
        Timothy-edwards.com/works
      </a>

      <button className="w-full border border-[#810306] text-[#810306] text-sm font-medium py-2 rounded-md hover:bg-[#fff5f5] transition">
        View profile
      </button>
    </div>
  );
}
