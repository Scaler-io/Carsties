"use client";

import { useParamsStore } from "@/hooks/useParamsStore";
import React, { useState } from "react";
import { FaSearch } from "react-icons/fa";

const SearchBar = () => {
  const setParams = useParamsStore((state) => state.setParams);
  const setSearchValue = useParamsStore((state) => state.setSearchValue);
  const searchValue = useParamsStore((state) => state.searchValue);

  const onChange = (event: any) => {
    setSearchValue(event.target.value);
  };

  const search = () => {
    setParams({ searchTerm: searchValue });
  };

  return (
    <div className="flex w-[50%] items-center border-2 rounded-full py-2 border-sm">
      <input
        type="text"
        placeholder="Search for cars by make, model or color"
        className="
          flex-grow
          pl-5
          bg-transparent
          focus:outline-none
          border-transparent
          focus:border-transparent
          focus:ring-0
          text-sm
          text-gray-600
        "
        value={searchValue}
        onChange={onChange}
        onKeyDown={(e: any) => {
          if (e.key === "Enter") search();
        }}
      />
      <button onClick={search}>
        <FaSearch
          size={30}
          className="bg-accentBlue text-white rounded-full p-2 cursor-point mx-2"
        />
      </button>
    </div>
  );
};

export default SearchBar;
