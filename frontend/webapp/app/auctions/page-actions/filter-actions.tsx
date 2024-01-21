import { useParamsStore } from "@/hooks/useParamsStore";
import { Button } from "flowbite-react";
import React from "react";
import { filterButtons } from "../../models/filter-options";

const Filter = () => {
  const setParams = useParamsStore((state) => state.setParams);
  const filterBy = useParamsStore((state) => state.filterBy);

  return (
    <div>
      <span className="uppercase text-sm text-gray-500 mr-2">Filters</span>
      <Button.Group>
        {filterButtons.map(({ label, icon: Icon, value }) => (
          <Button
            key={value}
            onClick={() => setParams({ filterBy: value })}
            color={`${filterBy === value ? "info" : "gray"}`}
            className="focus:ring-0">
            <Icon className="mr-3 h-4" />
            {label}
          </Button>
        ))}
      </Button.Group>
    </div>
  );
};

export default Filter;
