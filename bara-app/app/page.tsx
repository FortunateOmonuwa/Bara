"use client";

import Navbar from "@/components/Navbar";
import Image from "next/image";


export default function HomePage() {


  return (
    <main className="min-h-screen bg-white flex flex-col relative">
      <Navbar />

      <section className="flex flex-col-reverse md:flex-row items-center justify-between px-6 md:px-16 py-12 md:py-20 flex-1 max-w-7xl mx-auto w-full">
        <div className="md:w-1/2 max-w-xl mt-10 md:mt-0 relative">
          <h1 className="[font-family:var(--font-lato)] text-3xl md:text-5xl font-semibold leading-tight text-barRedMain">
            Where <span className="italic text-[#810306]">Writers </span>
            and <span className="italic text-[#810306]">Producers</span>{" "}
            Collaborate To Bring Stories To Life
          </h1>
          <p className="mt-4 md:text-medium text-[#333740]">
            Bara connects screenwriters and producers in one <br />
            secure space.
          </p>

          <div className="mt-6 flex flex-col sm:flex-row sm:space-x-4 space-y-4 sm:space-y-0">
            {/* Create Account link*/}
            <a
              href="/auth/register"
              className="bg-[#800000] text-white font-medium px-12 py-3 rounded-md hover:bg-[#1a0000] text-center relative   transition-all duration-300 ease-in-out 
              hover:scale-105"
            >
              Create account
            </a>

            {/* Explore for free link*/}
            <a
              href="/auth/login"
              className="border border-[#800000] text-[#800000] font-medium px-12 py-3 rounded-sm text-center 
             transition-all duration-300 ease-in-out 
              hover:scale-105"
            >
              Explore for free
            </a>
          </div>
        </div>

        {/* Right Image */}
        <div className="md:w-1/2 flex justify-center md:justify-end mt-8 md:mt-0">
          <Image
            src="/hero-image.png"
            alt="Bara App Hero"
            width={500}
            height={400}
            className="w-full max-w-md h-auto object-contain"
          />
        </div>
      </section>
    </main>
  );
}
