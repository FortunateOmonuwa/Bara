"use client";
import { useEffect, useRef } from "react";

interface Props {
  onClose: () => void;
}

export default function CreateAccountDropdown({ onClose }: Props) {
  const dropdownRef = useRef<HTMLDivElement>(null);

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(e.target as Node)
      ) {
        onClose();
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [onClose]);

  return (
    <div
      ref={dropdownRef}
      className="
        w-72
        bg-white
        shadow-lg
        rounded-md
        border border-gray-200
        p-4
        z-50
      "
    >
      <div className="p-3 hover:bg-[#F5F5F5] rounded-md cursor-pointer [font-family:var(--font-lato)]">
        <h3 className="font-bold text-[#22242A]">I am a Producer</h3>
        <p className="text-sm text-[#333740]">
          I want to discover original scripts and hire talented writers to bring
          my ideas to life.
        </p>
      </div>
      <hr className="my-2 mx-4 border border-[#ABADB2]" />
      <div className="p-3 hover:bg-[#F5F5F5] rounded-md cursor-pointer">
        <h3 className="font-bold text-[#22242A]">I am a Writer</h3>
        <p className="text-sm text-[#333740]">
          I want to sell my scripts and get hired for story development.
        </p>
      </div>
    </div>
  );
}
